using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;

namespace ConsoleRpgEntities.Models.Characters
{
    public class Player : ITargetable, IPlayer
    {
        public int Experience { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public virtual IEnumerable<Ability> Abilities { get; set; }

        public virtual Equipment? Equipment { get; set; }

        public void Attack(ITargetable target)
        {
            // Player-specific attack logic
            Console.WriteLine($"{Name} attacks {target.Name} with a {Equipment?.Weapon}!");
        }

        public void UseAbility(IAbility ability, ITargetable target)
        {
            if (Abilities.Contains(ability))
            {
                ability.Activate(this, target);
            }
            else
            {
                Console.WriteLine($"{Name} does not have the ability {ability.Name}!");
            }
        }

        public class Weapon
        {
            public string Name { get; set; }
            public int AttackPower { get; set; }

            public Weapon(string name, int attackPower)
            {
                Name = name;
                AttackPower = attackPower;
            }
        }

        public class Armor
        {
            public string Name { get; set; }
            public int DefenseValue { get; set; }

            public Armor(string name, int defenseValue)
            {
                Name = name;
                DefenseValue = defenseValue;
            }
        }
    }
}
