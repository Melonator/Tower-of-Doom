using Microsoft.Xna.Framework;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Timers;
using Console = SadConsole.Console;

namespace TowerOfDoom.Entities
{
    public class Player : Actor
    {
        public int TauntCounter = 0;
        public bool Attacked = false;
        //Screens
        private static UI.DrawImageComponent gameover = new UI.DrawImageComponent("Art/GameOverScreen.png");
        private static UI.DrawImageComponent win = new UI.DrawImageComponent("Art/WinScreen.png");
        private static Console GameOverScreen;
        public int DeathScreen = 0;
        public int MarauderKills = 0;
        public Player(int glyph) : base(glyph)
        {
            MaxHealth = 45;
            Health = MaxHealth;
            Attack = 10;
            AttackChance = 50;
            Defense = 15;
            DefenseChance = 10;
            TauntChance = 40;
            Name = "The Slayer";
        }
        public void ShowDeathScreen()
        {
            Timer aTimer;
            aTimer = new System.Timers.Timer(3000);
            aTimer.Elapsed += ResetGame;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
            GameLoop.UIManager.MapWindow.Hide();
            gameover.PositionOffset = new Point(1, 1);
            gameover.PositionMode = UI.DrawImageComponent.PositionModes.Pixels;
            GameOverScreen = new Console(GameLoop.GameWidth, GameLoop.GameHeight);
            GameOverScreen.Components.Add(gameover);
            SadConsole.Global.CurrentScreen = GameOverScreen;
        }

        public void ShowWinScreen()
        {
            Timer aTimer;
            aTimer = new System.Timers.Timer(3000);
            aTimer.Elapsed += ResetGame;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
            GameLoop.UIManager.MapWindow.Hide();
            win.PositionOffset = new Point(1, 1);
            win.PositionMode = UI.DrawImageComponent.PositionModes.Pixels;
            GameOverScreen = new Console(GameLoop.GameWidth, GameLoop.GameHeight);
            GameOverScreen.Components.Add(win);
            SadConsole.Global.CurrentScreen = GameOverScreen;
        }
        private static void ResetGame(Object source, ElapsedEventArgs e)
        {
            GameOverScreen.Components.Remove(gameover);
            GameLoop.UIManager.Clear();
            GameLoop.Init();           
            GameLoop.Counter = 0;
        }
    }
}
