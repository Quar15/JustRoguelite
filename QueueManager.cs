using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JustRoguelite.Utility;
using JustRoguelite.Characters;

namespace JustRoguelite
{
    internal class QueueManager
    {
        private List<CharacterBase> _queue = new();
        private int _queueIndex;

        public List<CharacterBase> GetQueue() { return _queue; }

        private void PrintQueue()
        {
            int queueLength = _queue.Count;
            Logger.Instance().Info($"Queue [N: {queueLength}]", "QueueManager.PrintQueue()");
            for (int i = 0; i < queueLength; ++i)
            {
                Logger.Instance().Info($"\t- [{i}] - ID: {_queue[i].GetBattleID()}, Name: {_queue[i].GetName()}, Speed: {_queue[i].GetSpeed()}", "QueueManager.PrintQueue()");
            }
        }

        private void SortQueue()
        {
            _queue.Sort((a, b) => {
                int aSpeed = a.GetSpeed();
                int bSpeed = b.GetSpeed();
                if (aSpeed == bSpeed) 
                {
                    Random rnd = new();
                    return rnd.Next(-1, 1);
                }
                    

                return b.GetSpeed().CompareTo(a.GetSpeed());
            });
            Logger.Instance().Info("Queue Sorted", "QueueManager.SortQueue()");
            PrintQueue();
        }

        public void CreateQueue(List<CharacterBase> characters)
        {
            _queue.AddRange(characters);
            SortQueue();
        }

        public void RefreshQueue()
        {
            SortQueue();
            _queueIndex = 0;
        }

        public void ClearQueue()
        {
            _queue.Clear();
            Logger.Instance().Info("Queue Cleared", "QueueManager.ClearQueue()");
        }

        public bool TryToGetNextInQueue(ref CharacterBase? character)
        {
            int queueLength = _queue.Count;
            while (_queueIndex < queueLength)
            {
                if (_queue[_queueIndex].IsAlive() && character != _queue[_queueIndex])
                    break;

                _queueIndex++;
            }

            if (_queueIndex >= queueLength) return false;

            character = _queue[_queueIndex];
            Logger.Instance().Info($"Current character: {character.GetName()}", "QueueManager.TryToGetNextInQueue()");
            return true;
        }
    }
}
