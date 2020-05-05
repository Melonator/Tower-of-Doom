using System;
using TowerOfDoom.Entities;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using System.Collections.Generic;

namespace TowerOfDoom
{
    public class World
    {
        private int _mapWidth = 100;
        private int _mapHeight = 100;
        private TileBase[] _mapTiles;
        private int _maxRooms = 30;
        private int _minRoomSize = 10;
        private int _maxRoomSize = 15;
        public Map CurrentMap { get; set; }
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        public Player Player { get; set; }

        public World()
        {
            CreateMap();

            CreatePlayer();

            CreateMonsters();
        }
        private void CreateMap()
        {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            MapGenerator mapGen = new MapGenerator();
            CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }
        private void CreateMonsters()
        {
            List<Point> TakenLocation = new List<Point>();
            int impNum = 35;
            Random rndNum = new Random();
            for (int i = 0; i < impNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("An Imp", 2, 4, 25, 3, 50, 25, 12, 20);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove && !(TakenLocation.Contains(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width))))
                {
                    monsterPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                TakenLocation.Add(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width));
                CurrentMap.Add(newMonster);
            }

            int mancubusNum = 10;
            Random rndNum2 = new Random();
            for (int i = 0; i < mancubusNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Mancubus", 3, 9, 30, 5, 40, 30, 25, 20);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove && !(TakenLocation.Contains(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width))))
                {
                    monsterPosition = rndNum2.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                TakenLocation.Add(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width));
                CurrentMap.Add(newMonster);
            }

            int cacodemonNum = 10;
            Random rndNum3 = new Random();

            for (int i = 0; i < cacodemonNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Cacodemon", 4, 7, 50, 4, 30, 20, 10, 40);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove && !(TakenLocation.Contains(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width))))
                {
                    monsterPosition = rndNum3.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                TakenLocation.Add(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width));
                CurrentMap.Add(newMonster);
            }

            int pinkyNum = 10;
            Random rndNum4 = new Random();
            for (int i = 0; i < pinkyNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Pinky", 5, 5, 60, 20, 4, 20, 15, 80);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove && !(TakenLocation.Contains(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width))))
                {
                    monsterPosition = rndNum4.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                TakenLocation.Add(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width));
                CurrentMap.Add(newMonster);
            }

            int baronNum = 5;
            Random rndNum5 = new Random();
            for (int i = 0; i < baronNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Baron of Hell", 6, 9, 65, 10, 35, 0, 25, 35);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove && !(TakenLocation.Contains(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width))))
                {
                    monsterPosition = rndNum5.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                TakenLocation.Add(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width));
                CurrentMap.Add(newMonster);
            }

            int marauderNum = 2;
            Random rndNum6 = new Random();
            for (int i = 0; i < marauderNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("The Marauder", 7, 15, 50, 5, 50, 0, 45, 45);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove && !(TakenLocation.Contains(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width))))
                {
                    monsterPosition = rndNum6.Next(0, CurrentMap.Width * CurrentMap.Height);
                }
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                TakenLocation.Add(new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width));
                CurrentMap.Add(newMonster);
            }

        }
        private void CreatePlayer()
        {
            SadConsole.Global.FontDefault = normalSizedFont;
            Player = new Player(1);
            Player.Components.Add(new EntityViewSyncComponent());
            for (int i = 0; i < CurrentMap.Tiles.Length; i++)
            {
                if (!CurrentMap.Tiles[i].IsBlockingMove)
                {
                    Player.Position = SadConsole.Helpers.GetPointFromIndex(i, CurrentMap.Width);
                    break;
                }
            }
            CurrentMap.Add(Player);
        }
    }
}
