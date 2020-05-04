<<<<<<< HEAD
﻿using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using TowerOfDoom.UI;
using TowerOfDoom.Commands;
=======
﻿using Microsoft.Xna.Framework;
using SadConsole;
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627

namespace TowerOfDoom
{
    class GameLoop
    {

        public const int GameWidth = 80;
        public const int GameHeight = 25;
<<<<<<< HEAD
        public static Console MenuScreen;
        public static Console TutorialScreen;

        // Managers
        public static UIManager UIManager;
        public static CommandManager CommandManager;
=======
        public static int Counter;
        public static Console MenuScreen;
        // Managers
        public static UIManager UIManager;
        public static World World;

        private static ScrollingConsole _startingConsole;

        //Menu Image
        public static DrawImageComponent ImageComponent = new DrawImageComponent("Art/MenuScreen.png");
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627

        public static World World;
        public static int Counter = 0;
        public static DrawImageComponent imageComponent = new DrawImageComponent("Art/MenuScreen.png");
        public static DrawImageComponent tutorial = new DrawImageComponent("Art/MenuScreenTutorial.png");
        static void Main(string[] args)
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(GameWidth, GameHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Hook the update event that happens each frame so we can trap keys and respond.
            SadConsole.Game.OnUpdate = Update;

            // Start the game.
            SadConsole.Game.Instance.Run();

            //
            // Code here will not run until the game window closes.
            //

            SadConsole.Game.Instance.Dispose();
        }

        private static void Update(GameTime time)
        {
<<<<<<< HEAD
            if(Counter == 0)
            {
                if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    MenuScreen.Components.Remove(imageComponent);
                    tutorial.PositionOffset = new Point(1, 1);
                    tutorial.PositionMode = DrawImageComponent.PositionModes.Pixels;
                    TutorialScreen = new Console(GameWidth, GameHeight);
                    TutorialScreen.Components.Add(tutorial);
                    SadConsole.Global.CurrentScreen = TutorialScreen;
                    Counter++;
                
                }
            }
            else if (Counter == 1)
            {

                if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    CommandManager = new CommandManager();
=======
            if (Counter == 0)
            {
                if (Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627
                    UIManager = new UIManager();
                    World = new World();
                    UIManager.CreateConsoles();
                    UIManager.Init();
<<<<<<< HEAD
                    //Couldnt Find a way to separate the update function since its UIManager overrides it
                    Counter++;
                    //Clear Menu Screen
                    TutorialScreen.Components.Remove(tutorial);
=======
                    //Couldn't Find a way to separate the update function since its UIManager overrides it
                    Counter++;
                    //Clear Menu Screen
                    MenuScreen.Components.Remove(ImageComponent);
                    MenuScreen.Clear();
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627
                }
            }
        }

        private static void Init()
        {
<<<<<<< HEAD
            imageComponent.PositionOffset = new Point(1, 1);
            imageComponent.PositionMode = DrawImageComponent.PositionModes.Pixels;
            MenuScreen = new Console(GameWidth, GameHeight);
            MenuScreen.Components.Add(imageComponent);
            SadConsole.Global.CurrentScreen = MenuScreen;
=======
            ImageComponent.PositionOffset = new Point(1, 1);
            ImageComponent.PositionMode = DrawImageComponent.PositionModes.Pixels;
            MenuScreen = new Console(GameWidth, GameHeight);
            MenuScreen.Components.Add(ImageComponent);
            Global.CurrentScreen = MenuScreen;    
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627
        }
    }
}
