using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace JustRoguelite.Devtools.Terminal
{
    public class Keyboard
    {
        public ConcurrentQueue<KeyboardInput> KeypressQueue = new ConcurrentQueue<KeyboardInput>();
        public bool Exit { get; set; } = false;

        internal Keyboard()
        {
            Task.Run(ReadKeysAsync);
        }
        void ReadKeysAsync()
        {
            while (!Exit)
            {
                KeypressQueue.Enqueue(KeyboardInput.FromConsoleKeyInfo(Console.ReadKey(true)));
            }
        }
    }

}
