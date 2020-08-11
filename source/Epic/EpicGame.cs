using Abstract;
using System;
using System.Collections.Generic;
namespace Epic
{
    class EpicGame : Game
    {
        private string name;
        private string gameDirectory;
        private string launcher;
        private string executableURL;
        private bool incompleteInstall;

        public EpicGame()
        {
            this.name = null;
            this.gameDirectory = null;
            this.launcher = "Epic Games";
            this.executableURL = null;
            this.incompleteInstall = true;
        }

        public EpicGame(string displayName, string gameDirectory, string appName, bool incompleteInstall)
        {
            this.name = displayName;
            this.gameDirectory = gameDirectory;
            this.launcher = "Epic Games";
            this.executableURL = string.Format(@"com.epicgames.launcher://apps/{0}?action=launch&silent=true", appName);
            this.incompleteInstall = incompleteInstall;
        }

        public override string Name
        {
            get { return name; }
        }

        public override string GameDirectory
        {
            get { return gameDirectory; }
        }

        public override string Launcher
        {
            get { return launcher; }
        }

        public bool IncompleteInstall
        {
            get { return incompleteInstall; }
        }

        public string ExecutableURL
        {
            get { return executableURL; }
        }
    }
}
