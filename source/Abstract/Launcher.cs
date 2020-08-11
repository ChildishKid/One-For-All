using System;
using System.Collections.Generic;

namespace Abstract
{
    abstract class Launcher
    {
        public abstract void Launch(Game game);

        public abstract string Client { get; }
        public abstract string LauncherDirectory { get; }
        public abstract Boolean Installed { get; }
        public abstract List<Game> Games { get; }
    }
}
