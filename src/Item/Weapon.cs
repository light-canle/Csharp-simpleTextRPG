using System;
using System.Security.Authentication;
using Combat;
using VariousEntity;

namespace VariousItem
{
    public class Weapon : Equipable
    {
        public int RawMinDamage { get; set; }
        public int RawMaxDamage { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public DamageType DamageType { get; set; }
        public double CriticalChance { get; set; }
        public double CriticalPower { get; set; }
        public double Accuracy { get; set; }

        // ====================생성자====================
        public Weapon() : base("Air", 0, Position.Weapon)
        {
            RawMinDamage = 0;
            MinDamage = RawMinDamage;
            RawMaxDamage = 0;
            MaxDamage = RawMaxDamage;
            CriticalChance = 0;
            CriticalPower = 0;
            Accuracy = 0;
            DamageType = DamageType.Normal;
        }
        public Weapon(string name, int cost, int minDmg, int maxDmg, double critChance, double critPower, double accuracy, Quality quality) : base(name, cost, Position.Weapon)
        {
            RawMaxDamage = minDmg;
            MinDamage = RawMinDamage;
            RawMaxDamage = maxDmg;
            MaxDamage = RawMaxDamage;
            CriticalChance = critChance;
            CriticalPower = critPower;
            Accuracy = accuracy;
        }
        // ====================메소드====================
        // TODO : WeaponSkill로 이전
        /// <summary>
        /// 이 무기의 기본 대미지를 반환한다.
        /// </summary>
        public AttackInfo Attack()
        {
            Random rand = new Random();
            // 인챈트 반영
            UpdateEnchant();
            // 강화수치 반영
            int minDamage = MinDamage;
            int maxDamage = MaxDamage;

            int damage = 0;
            switch (rand.NextDouble())
            {
                // 치명타인 경우
                case double d when d <= CriticalChance:
                    damage = rand.Next((int)Math.Floor((double)maxDamage * CriticalPower), (int)Math.Floor((double)maxDamage * CriticalPower));
                    return new AttackInfo(true, true, damage, DamageType);
                // 일반 공격인 경우
                default:
                    damage = rand.Next(minDamage, maxDamage);
                    return new AttackInfo(true, false, damage, DamageType);
            }
        }

        public override Weapon Clone()
        {
            Weapon w = new Weapon();
            w.RawMaxDamage = RawMaxDamage;
            w.RawMinDamage = RawMinDamage;
            w.DamageType = DamageType;
            w.CriticalChance = CriticalChance;
            w.Accuracy = Accuracy;

            w.Position = Position;
            w.EnchantList = EnchantList.ConvertAll(en => new Enchantment(en.Type, en.Level));

            w.Cost = Cost;
            w.Name = Name;

            return w;
        }

        public override void Enchant(Enchantment enchant)
        {
            switch (enchant.Type)
            {
                case EnchantmentType.Sharpness:
                case EnchantmentType.AttackReinforcement:
                case EnchantmentType.Heavyness:
                case EnchantmentType.Accuracy:
                    base.Enchant(enchant);
                    break;

                default:
                    throw new ArgumentException("Weapon.Enchant() : 무기에 할 수 없는 인챈트입니다.");
            }
        }

        public override void UpdateEnchant()
        {
            RawMaxDamage = RawMaxDamage;
            RawMinDamage = RawMinDamage;
            DamageType = DamageType;
            CriticalChance = CriticalChance;
            Accuracy = Accuracy;
            base.UpdateEnchant();
        }
    }
}