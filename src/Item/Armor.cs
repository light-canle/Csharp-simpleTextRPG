using Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;

namespace VariousItem
{
    public class Armor : Equipable
    {
        public Stat Stat { get; set; }
        public Resistance Resistance { get; set; }

        // ====================생성자====================
        public Armor(string name, int cost, int ac, int mr,
            Position pos) : base(name, cost, pos)
        {
            Stat = new Stat();
            Stat.SetZero();
            Stat.BaseAC = ac;
            Stat.AC = ac;
            Stat.BaseMR = mr;
            Stat.MR = mr;
            Resistance = new Resistance();
        }

        public Armor(string name, int cost, Stat stat, Position pos, Resistance r)
            : base(name, cost, pos)
        {
            Stat = stat;
            Resistance = (Resistance)r.Clone();
        }

        public Armor(string name, int cost, int ac, int mr, Position pos, Resistance r)
            : base(name, cost, pos)
        {
            Stat = new Stat();
            Stat.SetZero();
            Stat.BaseAC = ac;
            Stat.AC = ac;
            Stat.BaseMR = mr;
            Stat.MR = mr;
            Resistance = (Resistance)r.Clone();
        }

        // ====================메소드====================
        public override Armor Clone()
        {
            Armor a = new Armor(Name, Cost, Stat.AC, Stat.MR, Position);
            a.Stat = (Stat)Stat.Clone();
            a.Resistance = (Resistance)Resistance.Clone();
            return a;
        }

        public override void Enchant(Enchantment enchant)
        {
            switch (enchant.Type)
            {
                case EnchantmentType.Hardness:
                case EnchantmentType.AntiMagic:
                    base.Enchant(enchant);
                    UpdateEnchant();
                    break;
                default:
                    throw new ArgumentException("Armor.Enchant() : 방어구에 할 수 없는 인챈트입니다.");
            }
            
        }

        public override void UpdateEnchant()
        {
            Stat.AC = Stat.BaseAC;
            Stat.MR = Stat.BaseMR;
            base.UpdateEnchant();
        }
    }
}
