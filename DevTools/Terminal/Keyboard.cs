using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace JustRoguelite.Devtools.Terminal
{
    // Main class responsible for handling keyboard input.
    //
    // On creation spawns a thread that reads input from the console,
    // parsers it and adds it to a queue.
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
