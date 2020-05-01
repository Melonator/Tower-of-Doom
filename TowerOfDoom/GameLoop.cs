using Microsoft.Xna.Framework;
using SadConsole;

namespace TowerOfDoom
{
    class GameLoop
    {

        public const int GameWidth = 80;
        public const int GameHeight = 25;
        public static int Counter;
        public static Console MenuScreen;
        // Managers
        public static UIManager UIManager;
        public static World World;

        private static ScrollingConsole _startingConsole;

        //Menu Image
        public static DrawImageComponent ImageComponent = new DrawImageComponent("Art/MenuScreen.png");

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
            if (Counter == 0)
            {
                if (Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    UIManager = new UIManager();
                    World = new World();
                    UIManager.CreateConsoles();
                    UIManager.Init();
                    //Couldn't Find a way to separate the update function since its UIManager overrides it
                    Counter++;
                    //Clear Menu Screen
                    MenuScreen.Components.Remove(ImageComponent);
                    MenuScreen.Clear();
                }
            }
        }
        private static void Init()
        {
            ImageComponent.PositionOffset = new Point(1, 1);
            ImageComponent.PositionMode = DrawImageComponent.PositionModes.Pixels;
            MenuScreen = new Console(GameWidth, GameHeight);
            MenuScreen.Components.Add(ImageComponent);
            Global.CurrentScreen = MenuScreen;    
        }
    }
}