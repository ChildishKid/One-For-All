using Abstract;
using Epic;
using General;
using Steam;
using System;
using System.Collections.Generic;

namespace OneForAll
{
    class Program
    {
        static void Main(string[] args)
        {
            Display.Config();
            Display.Title();

            SteamLauncher sl = new SteamLauncher();
            EpicLauncher el = new EpicLauncher();

            List<Game> games = new List<Game>();
            games.AddRange(sl.Games);
            games.AddRange(el.Games);

            Display.ListOfGames(games);

            int currentLineCursor = Console.CursorTop;
            int game;
            while (true)
            {
                try
                {
                    Console.Write("\n Option: ");
                    string option = Console.ReadLine();
                    game = int.Parse(option); 
                    if (game < 1 || game > games.Count)
                        throw new ApplicationException();
                    Console.WriteLine();
                    break;

                }
                catch
                {
                    Console.SetCursorPosition(0, currentLineCursor - 1);
                    for (int i = 0; i < 4; i++)
                        Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, currentLineCursor - 1);
                    Console.WriteLine("\nInvalid Entry!");
                    continue;
                }
            }

            if (games[game - 1].Launcher == "Steam")
                sl.Launch(games[game - 1]);
            else if (games[game - 1].Launcher == "Epic Games")
                el.Launch(games[game - 1]);

            Display.Countdown();
            Environment.Exit(0);
        }
    }
}
