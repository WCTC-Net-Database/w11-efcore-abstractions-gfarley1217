using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public virtual string? Weapon { get; set; }

        public virtual string? Armor { get; set; }

    }

}
