using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;

namespace ConsoleRpgEntities.Models.Characters;

public interface IPlayer
{
    int Id { get; set; }
    string Name { get; set; }
    IEnumerable<Ability> Abilities { get; set; }
    Equipment? Equipment { get; set; } // Changed from 'object' to 'Equipment?'
    void Attack(ITargetable target);
    void UseAbility(IAbility ability, ITargetable target);
}
