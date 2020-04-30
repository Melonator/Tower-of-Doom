using System;
using Microsoft.Xna.Framework;

namespace TowerOfDoom
{

    public abstract class Actor : SadConsole.Entities.Entity
    {
        private int _health;
        private int _maxHealth;

        public int Health { get { return _health; } set { _health = value; } }
        public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

        protected Actor(Color background, Int32 glyph, int width = 1, int height = 1) : base(width, height)
        {
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
        }

    }

    public class Player : Actor
    {
        public Player(Color background, int glyph) : base(background, glyph)
        {

        }

        public bool MoveBy(Point positionChange)
        {
            if (TowerOfDoom.GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
            {
                Position += positionChange;
                return true;
            }
            else
                return false;
        }
        public bool MoveTo(Point newPosition)
        {
            Position = newPosition;
            return true;
        }


    }
}