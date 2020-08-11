using Abstract;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Epic
{
    class EpicLauncher : Launcher
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private string client;
        private string launcherDirectory;
        private bool installed;
        private List<Game> games;

        public EpicLauncher()
        {
            client = "EpicGames";
            RegistryKey epicKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Epic Games");
            launcherDirectory = ((string)epicKey.OpenSubKey("EOS").GetValue("OverlayPath")).Replace("Extras/Overlay", "Binaries/Win32");

            if (launcherDirectory == null)
            {
                Logger.Info("EpicGames is not installed.");
                installed = false;
                launcherDirectory = null;
                games = null;
            }
            else
            {
                Logger.Info("EpicGames launcher detected.");
                installed = true;
                string manifestDirectory = @"C:\ProgramData\Epic\EpicGamesLauncher\Data\Manifests";
                try
                {
                    string[] files = Directory.GetFiles(manifestDirectory, "*.item");
                    games = new List<Game>();

                    foreach (string file in files)
                    {
                        Manifest content = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(file));
                        string displayName = content.DisplayName;
                        string installLocation = content.InstallLocation;
                        string appName = content.AppName;

                        // DLC (Assumption that DLC has no executable)
                        if (content.LaunchExecutable == "")
                            continue;


                        try
                        {
                            if (content.bIsIncompleteInstall == null)
                                throw new ApplicationException();
                            bool.Parse(content.bIsIncompleteInstall);
                        }
                        catch
                        {
                            Logger.Warn("Unable to include " + displayName + " (Corrupted Manifest)");
                            continue;
                        }

                        if (bool.Parse(content.bIsIncompleteInstall))
                        {
                            Logger.Warn("Unable to include " + displayName + " (Incomplete Installation)");
                            continue;
                        }

                            if (displayName != null && installLocation != null && appName != null)
                            games.Add(new EpicGame(displayName, installLocation, appName, bool.Parse(content.bIsIncompleteInstall)));
                        else
                            Logger.Warn("Unable to include " + displayName + " (Corrupted Manifest)");
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
            if (!(game.Launcher != "Epic Games" || game == null))
            {
                /**
                if (Process.GetProcessesByName("EpicGamesLauncher").Length == 0)
                {
                    Logger.Info("Staring EpicGames launcher...");
                    Process.Start(launcherDirectory + @"/EpicGamesLauncher.exe");
                }
                */

                try
                {
                    Logger.Info("Starting " + game.Name + "...");
                    Process.Start(((EpicGame)game).ExecutableURL);
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to lanch EpicGames game (" + e.Message + ").");
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