using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousItem;

namespace VariousEntity
{
    public class Player : ArmedEntity
    {
        public Dictionary<Item, int> Inventory { get; private set; }
        public Player(string name) : base(name)
        {
            Inventory = new Dictionary<Item, int>();
        }
    }
}
