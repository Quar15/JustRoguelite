using System;
using System.Runtime.InteropServices;

namespace JustRoguelite.Devtools.Terminal
{
    // This class is responsible for setting up the Windows console to support VT100 escape sequences.
    //
    // Uses the raw Win32 API to set the console mode.
    // See https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
    // for more information.
    //
    // For the correct behaviour of the console, the following settings are required:
    // On STDIN:
    // - ENABLE_VIRTUAL_TERMINAL_INPUT disabled
    // On STDOUT:
    // - ENABLE_VIRTUAL_TERMINAL_PROCESSING enabled (required for VT100 escape sequences)
    // - DISABLE_NEWLINE_AUTO_RETURN enabled (to control exactly when the cursor moves to the next line)
    // On STDERR:
    // - DISABLE_NEWLINE_AUTO_RETURN enabled (to control exactly when the cursor moves to the next line)
    internal class WinVTConsole
    {
        IntPtr InputHandle, OutputHandle, ErrorHandle;

        uint originalInputMode, originalOutputMode, originalErrorMode;

        public WinVTConsole()
        {
            // Get current input settings
            InputHandle = GetStdHandle(STD_INPUT_HANDLE);
            if (!GetConsoleMode(InputHandle, out uint mode))
            {
                throw new Exception($"Unable to get input console mode, error code: {GetLastError()}.");
            }
            // Save original settings
            originalInputMode = mode;

            // Update input settings
            if ((mode & ENABLE_VIRTUAL_TERMINAL_INPUT) != 0)
            {
                mode &= ~ENABLE_VIRTUAL_TERMINAL_INPUT;
                if (!SetConsoleMode(InputHandle, mode))
                {
                    throw new Exception($"Unable to set input console mode, error code: {GetLastError()}.");
                }
            }

            // Get output settings
            OutputHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(OutputHandle, out mode))
            {
                throw new Exception($"Unable to get output console mode, error code: {GetLastError()}.");
            }
            originalOutputMode = mode;

            // Update output settings
            if ((mode & (ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN)) == 0)
            {
                mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(OutputHandle, mode))
                {
                    throw new Exception($"Unable to set output console mode, error code: {GetLastError()}.");
                }
            }

            // Get error settings
            ErrorHandle = GetStdHandle(STD_ERROR_HANDLE);
            if (!GetConsoleMode(ErrorHandle, out mode))
            {
                throw new Exception($"Unable to get error console mode, error code: {GetLastError()}.");
            }
            originalErrorMode = mode;

            // Update error settings
            if ((mode & DISABLE_NEWLINE_AUTO_RETURN) == 0)
            {
                mode |= DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(ErrorHandle, mode))
                {
                    throw new Exception($"Unable to set error console mode, error code: {GetLastError()}.");
                }
            }
        }

        // Restores the original console settings.
        public void Cleanup()
        {
            if (!SetConsoleMode(InputHandle, originalInputMode))
            {
                throw new Exception($"Unable to restore input console mode, error code: {GetLastError()}.");
            }
            if (!SetConsoleMode(OutputHandle, originalOutputMode))
            {
                throw new Exception($"Unable to restore output console mode, error code: {GetLastError()}.");
            }
            if (!SetConsoleMode(ErrorHandle, originalErrorMode))
            {
                throw new Exception($"Unable to restore error console mode, error code: {GetLastError()}.");
            }
        }

        // Windows data types: https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types
        // Console functions: https://learn.microsoft.com/en-us/windows/console/console-functions

        // https://learn.microsoft.com/en-us/windows/console/getstdhandle#parameters
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const int STD_ERROR_HANDLE = -12;

        // https://docs.microsoft.com/en-us/windows/console/getconsolemode#parameters
        const uint ENABLE_PROCESSED_INPUT = 0x0001;
        const uint ENABLE_INPUT_LINE = 0x0002;
        const uint ENABLE_ECHO_OUTPUT = 0x0004;
        const uint ENABLE_WINDOW_INPUT = 0x0008;
        const uint ENABLE_MOUSE_INPUT = 0x0010;
        const uint ENABLE_INSERT_MODE = 0x0020;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        // https://docs.microsoft.com/en-us/windows/console/setconsolemode#parameters
        const uint ENABLE_PROCESSED_OUTPUT = 0x0001;
        const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
        const uint ENABLE_LVB_GRID_WORLDWIDE = 0x0010;


        // https://learn.microsoft.com/en-us/windows/console/getstdhandle
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        // https://learn.microsoft.com/en-us/windows/console/getconsolemode
        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        // https://learn.microsoft.com/en-us/windows/console/setconsolemode
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        // https://learn.microsoft.com/en-us/windows/win32/api/errhandlingapi/nf-errhandlingapi-getlasterror
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
    }
}
