using System;

namespace JustRoguelite.Devtools.Terminal
{
    public struct KeyboardInput
    {
        public ConsoleKey Key { get; }
        public bool IsAlt { get; }
        public bool IsCtrl { get; }
        public bool IsShift { get; }

        public KeyboardInput(ConsoleKey key, bool isAlt = false, bool isCtrl = false, bool isShift = false)
        {
            Key = key;
            IsAlt = isAlt;
            IsCtrl = isCtrl;
            IsShift = isShift;
        }

        public static KeyboardInput FromConsoleKeyInfo(ConsoleKeyInfo keyInfo)
        {
            return new KeyboardInput(keyInfo.Key,
                keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt),
                keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control),
                keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
            );
        }

        public override string ToString()
        {
            string mods = $"{(IsAlt ? "A" : "")}{(IsCtrl ? "C" : "")}{(IsShift ? "S" : "")}";
            return $"<{mods}{(mods.Length > 0 ? "-" : "")}{Key}>";
        }
    }
}
