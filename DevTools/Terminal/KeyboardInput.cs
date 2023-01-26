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

        // public static Input FromConsoleKey(ConsoleKey key)
        // {
        //     return new Input(key, false, false, false);
        // }

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

        // public static Input FromConsoleKeyInfo(ConsoleKeyInfo keyInfo, bool isAlt, bool isCtrl, bool isShift)
        // {
        //     return new Input(keyInfo.Key, isAlt, isCtrl, isShift);
        // }

        // public static Input FromConsoleKeyInfo(ConsoleKeyInfo keyInfo, ConsoleModifiers modifiers)
        // {
        //     return new Input(keyInfo.Key, modifiers.HasFlag(ConsoleModifiers.Alt), modifiers.HasFlag(ConsoleModifiers.Control), modifiers.HasFlag(ConsoleModifiers.Shift));
        // }

        // public static Input FromConsoleKeyInfo(ConsoleKeyInfo keyInfo, ConsoleModifiers modifiers, bool isAlt, bool isCtrl, bool isShift)
        // {
        //     return new Input(keyInfo.Key, modifiers.HasFlag(ConsoleModifiers.Alt) || isAlt, modifiers.HasFlag(ConsoleModifiers.Control) || isCtrl, modifiers.HasFlag(ConsoleModifiers.Shift) || isShift);
        // }

        // public static Input FromConsoleKeyInfo(ConsoleKeyInfo keyInfo, bool isAlt, bool isCtrl, bool isShift, ConsoleModifiers modifiers)
        // {
        //     return new Input(keyInfo.Key, modifiers.HasFlag(ConsoleModifiers.Alt) || isAlt, modifiers.HasFlag(ConsoleModifiers.Control) || isCtrl, modifiers.HasFlag(ConsoleModifiers.Shift) || isShift);
        // }

        // public static Input FromConsoleKeyInfo(ConsoleKeyInfo keyInfo, ConsoleModifiers modifiers, bool isAlt, bool isCtrl, bool isShift, ConsoleModifiers modifiers2)
        // {
        //     return new Input(keyInfo.Key, modifiers.HasFlag(ConsoleModifiers.Alt) || isAlt || modifiers2.HasFlag(ConsoleModifiers.Alt), modifiers.HasFlag(ConsoleModifiers.Control) || isCtrl || modifiers2.HasFlag(ConsoleModifiers.Control), modifiers.HasFlag(ConsoleModifiers.Shift) || isShift || modifiers2.HasFlag(ConsoleModifiers.Shift));
        // }

    }
}