using System;
using SadConsole;
using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    public abstract class TileBase : Cell
    {
        public bool IsBlockingMove;
        public bool IsBlockingLOS;
        public bool IsCorner;
        public string Name;

        protected TileBase(Color foreground, Color background, int glyph, bool blockingMove = false, bool blockingLOS = false, String name = "") : base(foreground, background, glyph)
        {
            IsBlockingMove = blockingMove;
            IsBlockingLOS = blockingLOS;
            Name = name;
        }
    }

    public class TileWall : TileBase
    {
        public TileWall(int glyph, bool corner = false, bool blocksMovement = true, bool blocksLOS = true, string n = "Wall") : base(Color.LightGray, Color.Transparent, glyph, blocksMovement, blocksLOS)
        {
            IsCorner = corner;
            Name = n;
        }
    }

    public class TileFloor : TileBase
    {
        public TileFloor(int glyph, bool blocksMovement = false, bool blocksLOS = false, string n = "Floor") : base(Color.DarkGray, Color.Transparent, glyph, blocksMovement, blocksLOS)
        {
            Name = n;
        }
    }

}
