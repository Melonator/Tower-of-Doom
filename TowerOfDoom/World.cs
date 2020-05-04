using System;
using TowerOfDoom.Entities;
using Microsoft.Xna.Framework;
<<<<<<< HEAD
using SadConsole.Components;
=======
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627

namespace TowerOfDoom
{
    // All game state data is stored in World
    // also creates and processes generators
    // for map creation
    public class World
    {
        // map creation and storage data
        private int _mapWidth = 100;
        private int _mapHeight = 100;
        private TileBase[] _mapTiles;
        private int _maxRooms = 30;
        private int _minRoomSize = 10;
        private int _maxRoomSize = 15;
<<<<<<< HEAD
=======
        public Point Spawn;
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627
        public Map CurrentMap { get; set; }
        public SadConsole.Font normalSizedFont = SadConsole.Global.LoadFont("Fonts/CustomTile.font.json").GetFont(SadConsole.Font.FontSizes.One);
        // player data
        public Player Player { get; set; }

        // Creates a new game world and stores it in
        // publicly accessible
        public World()
        {
            // Build a map
            CreateMap();

            // create an instance of player
            CreatePlayer();

            // spawn a bunch of monsters
            CreateMonsters();
        }
<<<<<<< HEAD

        // Create a new map using the Map class
        // and a map generator. Uses several 
        // parameters to determine geometry
=======
        private void SetSpawn()
        {
            for (int x = 0; x < _mapTiles.Length; x++)
            {
                for (int y = 0; y < _mapTiles.Length; y++)
                {
                    if (CurrentMap.IsTileWalkable(new Point(x, y)) && x > 30 && y > 30)
                    {
                        Spawn.X = x;
                        Spawn.Y = y;
                        break;
                    }
                }
            }
        }
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627
        private void CreateMap()
        {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentMap = new Map(_mapWidth, _mapHeight);
            MapGenerator mapGen = new MapGenerator();
            CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }

        // Create some random monsters
        // and drop them all over the map in
        // random places.
        private void CreateMonsters()
        {
            // number of monsters to create
            int impNum = 35;

            // random position generator
            Random rndNum = new Random();

            //spawn imps
            for (int i=0; i < impNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("An Imp",2, 5 ,25, 3, 50, 25, 10);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum.Next(0, CurrentMap.Width * CurrentMap. Height);
                }
                //monster position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }

            int mancubusNum = 10;

            // random position generator
            Random rndNum2 = new Random();

            //spawn mancubus
            for (int i = 0; i < mancubusNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Mancubus", 3, 8, 30, 5, 40, 30, 25);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum2.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                //monster position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }

            int cacodemonNum = 10;

            // random position generator
            Random rndNum3 = new Random();

            //spawn cacodemons
            for (int i = 0; i < cacodemonNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Cacodemon", 4, 7, 50, 2, 30, 20, 8);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum3.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                //monster position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }

            int pinkyNum = 10;

            // random position generator
            Random rndNum4 = new Random();

            //spawn pinkies
            for (int i = 0; i < pinkyNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Pinky", 5, 5, 60, 20, 4, 20, 15);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum4.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                //monster position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }

            int baronNum = 5;

            // random position generator
            Random rndNum5 = new Random();

            //spawn barons
            for (int i = 0; i < baronNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("A Baron of Hell", 6, 9, 65, 10, 35, 0, 30);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum5.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                //monster position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }

            int marauderNum = 2;

            // random position generator
            Random rndNum6 = new Random();

            //spawn two marauders
            for (int i = 0; i < marauderNum; i++)
            {
                int monsterPosition = 0;
                Monster newMonster = new Monster("The Marauder", 7, 15, 50, 5, 50, 0, 45);
                newMonster.Components.Add(new EntityViewSyncComponent());
                while (CurrentMap.Tiles[monsterPosition].IsBlockingMove)
                {
                    // pick a random spot on the map
                    monsterPosition = rndNum6.Next(0, CurrentMap.Width * CurrentMap.Height);
                }

                //monster position
                newMonster.Position = new Point(monsterPosition % CurrentMap.Width, monsterPosition / CurrentMap.Width);
                CurrentMap.Add(newMonster);
            }

        }

        // Create a player using the Player class
        // and set its starting position
        private void CreatePlayer()
        {
            SadConsole.Global.FontDefault = normalSizedFont;
            Player = new Player(1);
            Player.Components.Add(new EntityViewSyncComponent());
<<<<<<< HEAD
            // Place the player on the first non-movement-blocking tile on the map
            for (int i = 0; i < CurrentMap.Tiles.Length; i++)
            {
                if (!CurrentMap.Tiles[i].IsBlockingMove)
                {
                    // Set the player's position to the index of the current map position
                    Player.Position = SadConsole.Helpers.GetPointFromIndex(i, CurrentMap.Width);
                    break;
                }
            }
=======
            SetSpawn();
            Player.Position = new Point(Spawn.X, Spawn.Y);
            // Add the ViewPort sync Component to the player
>>>>>>> eaaeadc3173a4681415a4afa4415a6f0067d2627

            // add the player to the global EntityManager's collection of Entities
            CurrentMap.Add(Player);
        }
    }
}
