namespace TowerOfDoom.Entities
{
    public class Player : Actor
    {
        public int TauntCounter = 0;
        public Player(int glyph) : base(glyph)
        {
            Health = 45;
            Attack = 10;
            AttackChance = 50;
            Defense = 12;
            DefenseChance = 10;
            TauntChance = 40;
            Name = "The Slayer";
        }
    }
}
