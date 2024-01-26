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
        public Player(string name, int hp= 20, int mp = 5, int strength = 5, 
            int agility = 5, int spell = 5, int talent = 5, int ac = 0, int mr = 0) : 
            base(name, hp, mp, strength, agility, spell, talent, ac, mr)
        {
            Inventory = new Dictionary<Item, int>();
        }
        public void AddItem(Item item, int count = 1)
        {
            Inventory.Add(item, count);
        }

        public void RemoveItem(Item item, int count = 1)
        {
            int itemCount = Inventory[item];
            if (itemCount > count) 
            {
                Inventory[item] -= count;
            }
            else
            {
                Inventory.Remove(item);
            }
        }
    }
}
