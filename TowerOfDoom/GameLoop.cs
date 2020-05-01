using Microsoft.Xna.Framework;
using SadConsole;

namespace TowerOfDoom
{
    class GameLoop
    {

        public const int GameWidth = 80;
        public const int GameHeight = 25;
        public static int Counter = 0;
        public static Console MenuScreen;
        // Managers
        public static UIManager UIManager;
        public static World World;

        private static ScrollingConsole startingConsole;

        //Menu Image
        public static DrawImageComponent imageComponent = new DrawImageComponent("Art/MenuScreen.png");

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
                if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    UIManager = new UIManager();
                    World = new World();
                    UIManager.CreateConsoles();
                    UIManager.Init();
                    //Couldnt Find a way to separate the update function since its UIManager overrides it
                    Counter++;
                    //Clear Menu Screen
                    MenuScreen.Components.Remove(imageComponent);
                    MenuScreen.Clear();
                }
            }
        }
        private static void Init()
        {
            imageComponent.PositionOffset = new Point(1, 1);
            imageComponent.PositionMode = DrawImageComponent.PositionModes.Pixels;
            MenuScreen = new Console(GameWidth, GameHeight);
            MenuScreen.Components.Add(imageComponent);
            SadConsole.Global.CurrentScreen = MenuScreen;    
        }
    }
}