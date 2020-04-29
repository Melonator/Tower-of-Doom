using Microsoft.Xna.Framework;
using SadConsole;
using world;
using UI;

namespace TowerOfDoom
{
    class GameLoop
    {

        public const int GameWidth = 80;
        public const int GameHeight = 25;

        // Managers
        public static UIManager UIManager;
        public static World World;

        private static ScrollingConsole startingConsole;

        static void Main(string[] args)
        {

            SadConsole.Game.Create(GameWidth, GameHeight);

            SadConsole.Game.OnInitialize = Init;

            SadConsole.Game.OnUpdate = Update;

            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time)
        {

        }
        private static void Init()
        {
            //loads font          


            UIManager = new UIManager();

            // Build the world!
            World = new World();

            // Now let the UIManager create its consoles
            // so they can use the World data
            UIManager.CreateConsoles();
            UIManager.Init();

        }
    }
}