using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;

namespace ConsoleRpgEntities.Models.Characters
{
    public class Player : ITargetable, IPlayer
    {
        public int Experience { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Default value to avoid null warnings
        public int Health { get; set; }
        public virtual IEnumerable<Ability> Abilities { get; set; } = new List<Ability>(); // Default empty list
        public virtual Equipment? Equipment { get; set; } // Nullable to allow for no equipment

        public void Attack(ITargetable target)
        {
            // Player-specific attack logic
            Console.WriteLine($"{Name} attacks {target.Name} with a {Equipment?.Weapon ?? "fists"}!");
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

