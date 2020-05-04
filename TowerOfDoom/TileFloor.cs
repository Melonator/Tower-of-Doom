using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    // TileFloor is based on TileBase
    // Floor tiles to be used in maps.
    public class TileFloor : TileBase
    {
        // Default constructor
        // Floors are set to allow movement and line of sight by default
        // and have a dark gray foreground and a transparent background
        // represented by the . symbol
     
        public TileFloor(int glyph, bool blocksMovement = false, bool blocksLOS = false) : base(Color.DarkGray, Color.Transparent, glyph, blocksMovement, blocksLOS)
        {
            Name = "Floor";

        }
    }
}
