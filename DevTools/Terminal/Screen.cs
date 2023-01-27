using System;
using System.Text;

namespace JustRoguelite.Devtools.Terminal
{
    // ANSI cheatsheet
    // https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797
    public class Screen
    {
        int cols, rows, left, top;

        public int Cols => cols;
        public int Rows => rows;
        public int Left => left;
        public int Top => top;

        int colsOrig, rowsOrig;

        // Cursor column and row
        int ccol, crow;

        bool[] dirtyRows = new bool[0];
        // Row, Column, (utf8 char, ColScheme, Dirty)
        int[,,] contents = new int[0, 0, 3];
        public int[,,] Contents => contents;

        public ColorScheme CurrentColors;

        WinVTConsole? WinConsole = null;
        public bool IsWinPlatform => WinConsole != null;

        bool isInitialized = false;
        public bool IsInitialized => isInitialized;

        public Screen()
        {
            var p = Environment.OSVersion.Platform;
            if (p == PlatformID.Win32NT || p == PlatformID.Win32S || p == PlatformID.Win32Windows)
            {
                WinConsole = new WinVTConsole();
            }
        }
        public void Init()
        {
            if (isInitialized)
            {
                return;
            }

            // Save cursor position
            Console.Out.Write("\x1b[s");

            Console.Out.Flush();

            // Console.TreatControlCAsInput = true;

            colsOrig = Console.WindowWidth;
            rowsOrig = Console.WindowHeight;

            ccol = 0;
            crow = 0;

            CurrentColors = ColorScheme.Default;

            if (cols == 0 || rows == 0)
            {
                cols = colsOrig;
                rows = rowsOrig;
            }

            ResizeScreen();
            // Enable alternate screen buffer
            Console.Out.Write("\x1b[?1049h");
            // Hide cursor
            Console.Out.Write("\x1b[?25l");
            Clear();

            contents = new int[rows, cols, 3];
            dirtyRows = new bool[rows];
            try
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        contents[r, c, 0] = ' ';
                        contents[r, c, 1] = 0;
                        contents[r, c, 2] = 0;
                        dirtyRows[r] = true;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            isInitialized = true;
        }

        public void SetScreenSize(int width, int height)
        {
            cols = width;
            rows = height;
        }

        public void ExchangeColors(ref ColorScheme cs)
        {
            (cs, CurrentColors) = (CurrentColors, cs);
        }

        public void Move(int col, int row)
        {
            ccol = col;
            crow = row;
        }

        public void AddChar(char c, ColorScheme cs)
        {
            if (ccol >= cols || crow >= rows)
            {
                return;
            }

            contents[crow, ccol, 0] = c;
            contents[crow, ccol, 1] = cs;
            contents[crow, ccol, 2] = 1;
            dirtyRows[crow] = true;

            ccol++;
        }

        public void AddChar(char c)
        {
            AddChar(c, CurrentColors);
        }

        public void AddString(string s, ColorScheme cs)
        {
            foreach (var c in s)
            {
                AddChar(c, cs);
            }
        }

        public void AddString(string s)
        {
            foreach (var c in s)
            {
                AddChar(c);
            }
        }

        public void Clear()
        {
            if (Rows > 0)
            {
                // Console.Clear();
                // Console.Out.Write("\x1b[2J");
                if (contents.Length == Rows * Cols * 3)
                {
                    for (int row = 0; row < Rows; row++)
                    {
                        for (int col = 0; col < Cols; col++)
                        {
                            contents[row, col, 0] = ' ';
                            contents[row, col, 1] = 0;
                            contents[row, col, 2] = 1;
                            dirtyRows[row] = true;
                        }
                    }
                }
            }
        }

        public void UpdateScreen()
        {
            if (Console.WindowHeight == 0 || contents.Length != Rows * Cols * 3)
            {
                return;
            }

            int top = Top;
            int left = Left;
            int rows = Rows;
            int cols = Cols;

            StringBuilder output = new StringBuilder();
            int lastCol = -1;

            Console.CursorVisible = false;

            for (int row = top; row < rows; row++)
            {
                if (!dirtyRows[row])
                {
                    continue;
                }

                dirtyRows[row] = false;
                output.Clear();
                for (int col = left; col < cols; col++)
                {
                    if (!SetCursorPosition(col, row))
                    {
                        return;
                    }
                    lastCol = -1;
                    int outputWidth = 0;
                    for (; col < cols; col++)
                    {
                        if (contents[row, col, 2] == 0)
                        {
                            if (output.Length > 0)
                            {
                                SetVirtualCursorPosition(lastCol, row);
                                Console.Write(output);
                                output.Clear();
                                lastCol += outputWidth;
                                outputWidth = 0;
                            }
                            else if (lastCol == -1)
                            {
                                lastCol = col;
                            }
                            if (lastCol + 1 < cols)
                            {
                                lastCol++;
                            }
                            continue;
                        }

                        if (lastCol == -1)
                        {
                            lastCol = col;
                        }
                        int attr = contents[row, col, 1];
                        output.Append(GetANSIColorString(attr != 0 ? attr : CurrentColors));
                        output.Append((char)contents[row, col, 0]);
                        outputWidth++;

                        contents[row, col, 1] = 0;
                        contents[row, col, 2] = 0;
                    }
                }
                if (output.Length > 0)
                {
                    SetVirtualCursorPosition(lastCol, row);
                    Console.Write(output);
                }
            }
        }

        public void Exit()
        {
            Clear();
            Console.ResetColor();
            // Exit alternate screen buffer
            Console.Out.Write("\x1b[?1049l");
            // Show cursor
            Console.Out.Write("\x1b[?25h");
            // Restore screen size
            cols = colsOrig;
            rows = rowsOrig;
            ResizeScreen();
            // Restore saved cursor position
            Console.Out.Write("\x1b[u");

            Console.Out.Flush();

            if (IsWinPlatform)
            {
                WinConsole!.Cleanup();
            }

            isInitialized = false;
        }

        void ResizeScreen()
        {
            ccol = 0;
            crow = 0;
            if (Console.WindowHeight > 0)
            {
                try
                {
                    if (IsWinPlatform)
                    {
                        if (!SetCursorPosition(0, 0))
                        {
                            throw new Exception("SetCursorPosition failed");
                        }
                        Console.SetWindowPosition(0, 0);
                        Console.SetWindowSize(Cols, Rows);
                        Console.SetBufferSize(Cols, Rows);
                    }
                    else
                    {
                        Console.Out.Write($"\x1b[0;0;{Rows};{Cols}w");
                        Console.Out.Write($"\x1b[8;{Rows};{Cols}t");
                    }
                }
                catch (System.IO.IOException)
                {
                    return;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }
            }
        }

        string GetANSIColorString(int attr)
        {
            short fg = (short)((attr & 0xFF00) >> 8);
            short bg = (short)((attr & 0x00FF) + 10);

            return $"\x1b[{fg};{bg}m";
        }

        // Set the cursor position to the given column and row.
        bool SetCursorPosition(int col, int row)
        {
            try
            {
                Console.SetCursorPosition(col, row);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Set the ANSI cursor position to the given column and row (ansi indexes from 1).
        void SetVirtualCursorPosition(int lastCol, int row)
        {
            Console.Out.Write($"\x1b[{row + 1};{lastCol + 1}H");
            Console.Out.Flush();
        }
    }
}
