using Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;


namespace General
{
    class Display
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        public static void Config()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }

        public static void Title()
        {
            Console.WriteLine(@"   ___               _____               _    _ _ ");
            Console.WriteLine(@"  / _ \ _ __   ___  |  ___|__  _ __     / \  | | |");
            Console.WriteLine(@" | | | | '_ \ / _ \ | |_ / _ \| '__|   / _ \ | | |");
            Console.WriteLine(@" | |_| | | | |  __/ |  _| (_) | |     / ___ \| | |");
            Console.WriteLine(@"  \___/|_| |_|\___| |_|  \___/|_|    /_/   \_\_|_|");
            Console.WriteLine("\n");
        }

        public static void ListOfGames(List<Game> games)
        {
            Console.WriteLine("\n === GAME LIST === \n");
            for (int i = 1; i <= games.Count; i++)
            {
                Console.WriteLine(string.Format(" {0}) - {1}", i, games[i - 1].Name));
            }
        }

        public static void Countdown()
        {
            Console.WriteLine();
            int currentLineCursor = Console.CursorTop;
            for (int i = 10; i >= 0; i--)
            {
                Console.SetCursorPosition(0, currentLineCursor);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
                Console.Write("\r Shutting down... (" + i + ")");

                Thread.Sleep(1000);
            }
        }
    }

}
