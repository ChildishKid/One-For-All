using Abstract;

namespace Steam
{
    class SteamGame : Game
    {
        private string name;
        private string gameDirectory;
        private string launcher;
        private string appid;

        public SteamGame()
        {
            this.name = null;
            this.gameDirectory = null;
            this.appid = null;
            this.launcher = "Steam";
        }

        public SteamGame(string name, string gameDirectory, string appid)
        {
            this.name = name;
            this.gameDirectory = gameDirectory;
            this.appid = appid;
            this.launcher = "Steam";
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

        public string AppID
        {
            get { return appid; }
        }
    }
}
