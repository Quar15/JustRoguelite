using System;

namespace JustRoguelite.Utility
{
    public enum LogType { NONE, CONSOLE, FILE };
    internal class Logger
    {
        private static Logger _instance;

        protected Logger() { }

        public static Logger Instance()
        {
            if (_instance == null) { _instance = new Logger(); }

            return _instance;
        }

        private void Log(string msg, string objectName, string msgType, ConsoleColor msgTypeColor)
        {
            if (Globals.LOG_TYPE != LogType.CONSOLE) return;

            ConsoleColor beforeFGColor = Console.ForegroundColor;
            Console.ForegroundColor = msgTypeColor;
            Console.Write($"@{msgType}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" - [{objectName}] - {msg}");
            Console.ForegroundColor = beforeFGColor;
        }

        public void Info(string msg, string objectName)
        {
            Log(msg, objectName, "INFO", ConsoleColor.Blue);
        }

        public void Warning(string msg, string objectName)
        {
            Log(msg, objectName, "WARNING", ConsoleColor.Yellow);
        }

        public void Error(string msg, string objectName)
        {
            Log(msg, objectName, "ERROR", ConsoleColor.Red);
        }
    }
}
