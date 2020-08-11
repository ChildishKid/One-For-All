using Abstract;
using Microsoft.Win32;
using NLog;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Steam
{
    class SteamLauncher : Launcher
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private string client;
        private string launcherDirectory;
        private Boolean installed;
        private List<Game> games;

        public SteamLauncher()
        {
            client = "Steam";
            RegistryKey steamKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Valve").OpenSubKey("Steam");
            launcherDirectory = (string)steamKey.GetValue("SteamPath");

            if (launcherDirectory == null)
            {
                Logger.Info("Steam is not installed.");
                installed = false;
                launcherDirectory = null;
                games = null;
            }
            else
            {
                Logger.Info("Steam launcher detected.");
                installed = true;
                try
                {
                    string[] files = Directory.GetFiles(launcherDirectory + "\\steamapps", "appmanifest*");
                    games = new List<Game>();

                    foreach (string file in files)
                    {
                        var kv = new KeyValue();
                        kv.ReadFileAsText(file);
                        string name = kv["name"].Value;
                        string installDir = kv["installdir"].Value;
                        string appID = kv["appid"].Value;

                        if (name != null && installDir != null && appID != null)
                            games.Add(new SteamGame(name, launcherDirectory + @"\steamapps\common\" + installDir, appID));
                        else
                            Logger.Warn("Unable to include " + name + " (Corrupted Manifest)");
                    }
                    Logger.Info("Detected " + games.Count + " " + client + " games installed.");
                }
                catch (Exception e)
                {
                    Logger.Error("Error loading installed games (" + e.Message + ")");
                    games = null;
                }

            }
        }

        public override void Launch(Game game)
        {
            if (!(game.Launcher != "Steam" || game == null))
            {
                try
                {
                    Logger.Info("Starting " + game.Name + "...");
                    System.Diagnostics.Process.Start(launcherDirectory + @"\steam.exe", "-applaunch " + ((SteamGame)game).AppID);
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to lanch Steam game (" + e.Message + ").");
                }
            }
            else
            {
                Logger.Error("Failed to launch game. Game is either invalid or being launched from incorrect launcher");
            }
        }

        public override string Client
        {
            get { return client; }
        }

        public override string LauncherDirectory
        {
            get { return launcherDirectory; }
        }

        public override Boolean Installed
        {
            get { return installed; }
        }

        public override List<Game> Games
        {
            get { return games; }
        }
    }
}