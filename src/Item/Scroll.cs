using Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;

namespace VariousItem
{
    public abstract class Scroll : Consumable
    {
        public Scroll(string name, int cost) : base(name, cost) 
        {
            
        }
    }
    
    public class SkillScroll : Scroll
    {
        public DamageSkill Skill { get; protected set; }
        public SkillScroll(string name, int cost, DamageSkill skill) : base(name, cost)
        {
            Skill = skill;
        }
        public void Consume(Creature e)
        {
            if (e == null) { return; }

            AttackInfo info = Skill.Damage();
            e.ApplyDamage(info);
        }
    }
}
