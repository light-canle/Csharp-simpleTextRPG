using Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;

namespace VariousItem
{
    public class Potion : Consumable
    {
        public Effect Effect { get; protected set; }

        public Potion(string name, int cost, Effect effect) : base(name, cost)
        {
            Effect = (Effect)effect.Clone();
        }

        /// <summary>
        /// 해당 포션을 사용한다.
        /// </summary>
        public override void Consume(Creature e)
        {
            e.AddEffect(Effect);
        }

        public override Potion Clone()
        {
            return new Potion(Name, Cost, Effect);
        }
    }
}
