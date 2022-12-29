using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRoguelite.Utility
{
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
            if (!Globals.DEBUG_LOGS) return;

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
