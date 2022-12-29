using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JustRoguelite.Characters;
using JustRoguelite.Utility;
using JustRoguelite.Skills;

namespace JustRoguelite
{
    internal class BattleManager
    {
        private List<CharacterBase> _playerCharacters = new();
        private List<CharacterBase> _enemyCharacters = new();

        private QueueManager _queueManager;
        // private UIManager _uiManager;
        private CharacterBase? _currentCharacter;

        private bool _canExecuteTurn = false;

        // [SerializeField] private List<GameObject> battlePositions = new List<GameObject>();
        private const int NPLAYERPOSITIONS = 3;

        // public void SetUIManager(UIManager uiManager) { _uiManager = uiManager; }

        public BattleManager(QueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        private void SetupCharacter(CharacterBase character, int index)
        {
            // character.transform.position = battlePositions[index].transform.position;
            character.SetBattleID(index);
            character.turnExecuted += ExecuteTurn;
        }

        public void SetupBattle(List<CharacterBase> playerCharacters, List<CharacterBase> enemyCharacters)
        {
            _playerCharacters.AddRange(playerCharacters);
            _enemyCharacters.AddRange(enemyCharacters);

            // Set character positions
            for (int i = 0; i < playerCharacters.Count; i++)
                SetupCharacter(playerCharacters[i], i);

            for (int i = 0; i < enemyCharacters.Count; i++)
                SetupCharacter(enemyCharacters[i], i + NPLAYERPOSITIONS);

            _queueManager.CreateQueue(playerCharacters.Concat(enemyCharacters).ToList<CharacterBase>());

            EndTurn();
            // _uiManager.StartBattle();
        }

        public bool ExecuteTurn(CharacterBase castingCharacter, CharacterBase targetCharacter, Skill skill)
        {
            Logger.Instance().Info($"Trying to execute turn ({ castingCharacter.GetName() }, { targetCharacter.GetName() }, { skill.name })", "BattleManager.ExecuteTurn()");
            if (!_canExecuteTurn || _currentCharacter == null) return false;
            if (castingCharacter.GetBattleID() != _currentCharacter.GetBattleID()) return false;

            bool turnExecuted = skill.TryToExecute(castingCharacter, targetCharacter);
            if (turnExecuted) // Skill can fail
                EndTurn();

            Logger.Instance().Info($"Turn execution handled (Success: { turnExecuted })", "BattleManager.ExecuteTurn()");
            return turnExecuted;
        }

        private bool CheckIfBattleEnded()
        {
            int teamsAlive = 0;
            for (int i = 0; i < _playerCharacters.Count; ++i)
            {
                if (_playerCharacters[i].IsAlive())
                {
                    teamsAlive++;
                    break;
                }
            }

            for (int i = 0; i < _enemyCharacters.Count; ++i)
            {
                if (_enemyCharacters[i].IsAlive())
                {
                    teamsAlive++;
                    break;
                }
            }

            return !(teamsAlive == 2);
        }

        public void EndTurn()
        {
            _canExecuteTurn = false;
            Logger.Instance().Info("<b>Turn ended</b>", "BattleManager.EndTurn()");
            if (CheckIfBattleEnded())
            {
                EndBattle();
                return;
            }
            // Get next character in queue
            if (!_queueManager.TryToGetNextInQueue(ref _currentCharacter))
            {
                _queueManager.RefreshQueue();
                _queueManager.TryToGetNextInQueue(ref _currentCharacter);
            };
            // Update UI
            // _uiManager.battleUIManager.EndTurn(_currentCharacter, _playerCharacters, _enemyCharacters);
            _canExecuteTurn = true;

            if (_currentCharacter != null && !_currentCharacter.GetIsPlayer())
                _currentCharacter.ExecuteTurn();
        }

        public void EndBattle()
        {
            _queueManager.ClearQueue();
        }
    }
}
