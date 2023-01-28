using System;

namespace JustRoguelite.Utility
{
    public enum LogType { NONE, CONSOLE, FILE };
    internal class Logger
    {
        private static Logger _instance;

        private Queue<string> _logs = new();

        protected Logger() { }

        public static Logger Instance()
        {
            if (_instance == null) { _instance = new Logger(); }

            return _instance;
        }

        private void Log(string msg, string objectName, string msgType, ConsoleColor msgTypeColor)
        {
            switch (Globals.LOG_TYPE) 
            {
                case LogType.NONE:
                    return;

                case LogType.CONSOLE:
                    ConsoleColor beforeFGColor = Console.ForegroundColor;
                    Console.ForegroundColor = msgTypeColor;
                    Console.Write($"@{msgType}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($" - [{objectName}] - {msg}");
                    Console.ForegroundColor = beforeFGColor;
                    break;

                case LogType.FILE:
                    _logs.Enqueue($"@{msgType} - [{objectName}] - {msg}");
                    if(_logs.Count > Globals.MAX_LOGS_N)
                        _logs.Dequeue();
                    break;

                default:
                    return;
            }            
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

        public void SaveLogs(string fileName = "./logs.txt") 
        {
            using StreamWriter file = new(fileName);
            
            foreach(var log in _logs) 
            {
                file.WriteLine(log);
            }
        }
    }
}
