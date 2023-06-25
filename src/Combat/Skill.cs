using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;
using VariousItem;

namespace Combat
{
    public class Skill
    {
        public string Name { get; protected set; }
        public DamageType? DamageType { get; set; }

        // ====================생성자====================
        public Skill(string name, DamageType type)
        {
            Name = name;
            DamageType = type;
        }
    }

    public class DamageSkill : Skill
    {
        public int? RawMinDamage { get; protected set; }
        public int? RawMaxDamage { get; protected set; }
        public double? CriticalChance { get; protected set; }
        public double? Accuracy { get; protected set; }
        public DamageSkill(string name, int min, int max, double crit, double acc,
        DamageType type = Combat.DamageType.Normal) : base(name, type)
        {
            RawMinDamage = min;
            RawMaxDamage = max;
            CriticalChance = crit;
            Accuracy = acc;
            DamageType = type;
        }

        /// <summary>
        /// 이 스킬의 대미지를 리턴한다.
        /// Normal, Magic, Weapon, Special(대미지가 있음)일 때만 사용한다.
        /// </summary>
        public virtual AttackInfo Damage()
        {
            Random r = new Random();
            AttackInfo info;
            int damage;
            // 명중 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= Accuracy:
                    info = new AttackInfo(true, false, 0, DamageType.GetValueOrDefault());
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType.GetValueOrDefault());
                    return info;
            }
            // 크리티컬 히트 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage.GetValueOrDefault() * 1.6), RawMaxDamage.GetValueOrDefault() * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage.GetValueOrDefault(), (RawMaxDamage + 1).GetValueOrDefault());
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            return info;
        }
    }

    public class WeaponSkill : DamageSkill
    {
        public Weapon Weapon { get; private set; }
        public WeaponSkill(string name, Weapon weapon) :
            base(name, weapon.RawMinDamage, weapon.RawMaxDamage, weapon.CriticalChance,
                weapon.Accuracy, weapon.DamageType)
        {
            Weapon = weapon;
        }
        /// <summary>
        /// 이 스킬의 대미지를 리턴한다.
        /// </summary>
        public override AttackInfo Damage()
        {
            Random r = new Random();
            AttackInfo info;
            int damage;
            // 명중 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= Accuracy:
                    info = new AttackInfo(true, false, 0, DamageType.GetValueOrDefault());
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType.GetValueOrDefault());
                    return info;
            }
            // 크리티컬 히트 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage.GetValueOrDefault() * 1.6), RawMaxDamage.GetValueOrDefault() * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage.GetValueOrDefault(), (RawMaxDamage + 1).GetValueOrDefault());
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            return info;
        }
    }

    

    public class MagicSkill : DamageSkill
    {
        public int level { get; private set; }
        public int ManaCost { get; private set; }
        public Creature Conjurer { get; private set; }
        public MagicSkill(string name, int min, int max, double crit, double acc, DamageType type, Creature conjurer)
            : base(name, min, max, crit, acc, type)
        {
            Conjurer = conjurer;
        }
        /// <summary>
        /// 이 스킬의 대미지를 리턴한다.
        /// </summary>
        public override AttackInfo Damage()
        {
            Random r = new Random();
            AttackInfo info;
            int damage;
            // 명중 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= Accuracy:
                    info = new AttackInfo(true, false, 0, DamageType.GetValueOrDefault());
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType.GetValueOrDefault());
                    return info;
            }
            // 크리티컬 히트 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage.GetValueOrDefault() * 1.6), RawMaxDamage.GetValueOrDefault() * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage.GetValueOrDefault(), (RawMaxDamage + 1).GetValueOrDefault());
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            return info;
        }
    }

    public class EffectSkill : DamageSkill
    {
        public double EffectChance { get; set; }
        public Effect GiveEffect { get; set; }
        public EffectSkill(string name, int min, int max, double crit, double acc, double effchance, DamageType type, Effect give):
            base(name, min, max, crit, acc, type)
        {
            EffectChance = effchance;
            GiveEffect = give;
        }
        /// <summary>
        /// 이 스킬의 대미지를 리턴한다.
        /// </summary>
        public override AttackInfo Damage()
        {
            Random r = new Random();
            AttackInfo info;
            int damage;
            // 명중 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= Accuracy:
                    info = new AttackInfo(true, false, 0, DamageType.GetValueOrDefault());
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType.GetValueOrDefault());
                    return info;
            }
            // 크리티컬 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage.GetValueOrDefault() * 1.6), RawMaxDamage.GetValueOrDefault() * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage.GetValueOrDefault(), (RawMaxDamage + 1).GetValueOrDefault());
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            // 버프 / 디버프 적용
            switch (r.NextDouble())
            {
                case double d when d <= EffectChance:
                    info.Effect = GiveEffect;
                    // 크리티컬 히트 시 버프/디버프 1턴 추가
                    if (info.IsCritical)
                    {
                        info.Effect.Duration += 1;
                    }
                    break;

                default:
                    break;
            }
            return info;
        }
    }

    public class HealSkill : Skill
    {
        public HealSkill(string name, DamageType type) : base(name, type)
        {
        }
    }
}
