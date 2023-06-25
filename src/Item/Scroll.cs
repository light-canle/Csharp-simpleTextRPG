using Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;

namespace VariousItem
{
    public class Scroll : Consumable
    {
        public Scroll(string name, int cost) : base(name, cost) {}

        public override Scroll Clone()
        {
            return new Scroll(Name, Cost);
        }
    }

    public class SkillScroll : Scroll
    {
        public DamageSkill Skill { get; protected set; }
        public SkillScroll(string name, int cost, DamageSkill skill) : base(name, cost)
        {
            Skill = skill;
        }
        public void Consume(ref Creature e)
        {
            if (e == null) { return; }

            AttackInfo info = Skill.Damage();
            e.ApplyDamage(info);
        }
    }

    public abstract class InventoryScroll : Scroll
    {
        public InventoryScroll(string name, int cost) : base(name, cost) { }
        public abstract void Consume(ref Equipable i);
    }

    public class ScrollOfUpgrade : InventoryScroll
    {
        public ScrollOfUpgrade(string name, int cost) : base(name, cost) { }
        public override void Consume(ref Equipable i)
        {
            i.Reinforcement++;
        }
    }
}
