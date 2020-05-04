using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    // TileWall is based on TileBase
    // Walls are to be used in maps.
    public class TileWall : TileBase
    {
        // Default constructor
        // Walls are set to block movement and line of sight by default
        // and have a dark gray foreground and a transparent background
        // represented by the # symbol
        public TileWall(int glyph, bool corner = false, bool blocksMovement = true, bool blocksLOS = true) : base(Color.LightGray, Color.Transparent, glyph, blocksMovement, blocksLOS)
        {
            Name = "Wall";
            IsCorner = corner;
        }
    }
}
