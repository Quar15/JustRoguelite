using System;

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
        private CharacterBase? _currentCharacter;

        private bool _canExecuteTurn = false;

        private const int NPLAYERPOSITIONS = 3;

        public BattleManager(QueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        private void SetupCharacter(CharacterBase character, int index)
        {
            character.SetBattleID(index);
            character.turnExecuted += ExecuteTurn;
        }

        public void SetupBattle(CharactersList playerCharacters, CharactersList enemyCharacters) 
        {
            CharacterBase[] playerChar = playerCharacters.GetAll();
            CharacterBase[] enemyChar = enemyCharacters.GetAll();
            _playerCharacters.AddRange(playerChar);
            _enemyCharacters.AddRange(enemyChar);

            // Set character positions
            for (int i = 0; i < playerChar.Length; i++)
                SetupCharacter(playerChar[i], i);

            for (int i = 0; i < enemyChar.Length; i++)
                SetupCharacter(enemyChar[i], i + NPLAYERPOSITIONS);

            CharactersList allCharacters = new();
            allCharacters.Add(playerChar);
            allCharacters.Add(enemyChar);

            _queueManager.CreateQueue(allCharacters.GetAll());

            EndTurn();
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
            Logger.Instance().Info("Turn ended", "BattleManager.EndTurn()");
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
            _canExecuteTurn = true;

            if (_currentCharacter != null && _currentCharacter.GetCharacterType() == CharacterType.ENEMY)
                _currentCharacter.ExecuteTurn();
        }

        public void EndBattle()
        {
            _queueManager.ClearQueue();
        }
    }
}
