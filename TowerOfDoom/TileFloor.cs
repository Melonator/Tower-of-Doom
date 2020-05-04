using Microsoft.Xna.Framework;

namespace TowerOfDoom
{
    public class TileFloor : TileBase
    {
        public TileFloor(int glyph, bool blocksMovement = false, bool blocksLOS = false) : base(Color.DarkGray, Color.Transparent, glyph, blocksMovement, blocksLOS)
        {
            Name = "Floor";

        }
    }
}
