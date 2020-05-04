namespace TowerOfDoom.Entities
{
    public class Monster : Actor
    {
        public Monster(string name, int glyph, int attack, int attackchance, int defense, int defensechance, int tauntchance, int hp) : base(glyph)
        {
            Name = name;
            Attack = attack;
            AttackChance = attackchance;
            Defense = defense;
            DefenseChance = defensechance;
            TauntChance = tauntchance;
            Health = hp;
        }
    }
}
