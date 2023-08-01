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
        public int Capacity { get; private set; }
        public Player(string name) : base(name)
        {
            Inventory = new Dictionary<Item, int>();
        }
        public void AddItem(Item item, int count = 1)
        {
            Inventory.Add(item, count);
        }

        public void RemoveItem(Item item, int count = 1)
        {
            Inventory.Remove(item);
        }
    }
}
