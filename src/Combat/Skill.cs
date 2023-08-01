using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VariousEntity;
using VariousItem;

namespace Combat
{
    public class Skill : ICloneable
    {
        public string Name { get; protected set; }

        // ====================생성자====================
        public Skill(string name)
        {
            Name = name;
        }
        
        public virtual object Clone()
        {
            return new Skill(Name);
        }
    }

    public class DamageSkill : Skill
    {
        public int RawMinDamage { get; protected set; }
        public int RawMaxDamage { get; protected set; }
        public double CriticalChance { get; protected set; }
        public double Accuracy { get; protected set; }
        public DamageType DamageType { get; protected set; }

        /// <summary>
        /// 대미지를 주는 스킬의 생성자
        /// </summary>
        /// <param name="name">스킬 이름</param>
        /// <param name="min">최소 대미지</param>
        /// <param name="max">최대 대미지</param>
        /// <param name="crit">치명타 확률</param>
        /// <param name="acc">명중률</param>
        /// <param name="type">대미지 타입</param>
        public DamageSkill(string name, int min, int max, double crit, double acc,
        DamageType type = Combat.DamageType.Normal) : base(name)
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
                    info = new AttackInfo(true, false, 0, DamageType);
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType);
                    return info;
            }
            // 크리티컬 히트 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage * 1.6), RawMaxDamage * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage, (RawMaxDamage + 1));
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            return info;
        }

        public override object Clone()
        {
            return new DamageSkill(Name, RawMinDamage, RawMaxDamage,
                CriticalChance, Accuracy, DamageType);
        }
    }

    public class WeaponSkill : DamageSkill
    {
        public Weapon Weapon { get; private set; }
        public double Multiplier { get; private set; }
        public Stat Stat { get; set; }
        /// <summary>
        /// 무기 기반 스킬의 생성자
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="weapon">기반 무기</param>
        /// <param name="stat">스킬 사용자의 스탯</param>
        /// <param name="multiplier">공격력 계수</param>
        public WeaponSkill(string name, Weapon weapon, Stat stat, double multiplier) :
            base(name, (int)(weapon.RawMinDamage * multiplier), (int)(weapon.RawMaxDamage * multiplier), 
                weapon.CriticalChance, weapon.Accuracy, weapon.DamageType)
        {
            Weapon = weapon.Clone();
            Stat = (Stat)stat.Clone();
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
                    info = new AttackInfo(true, false, 0, DamageType);
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType);
                    return info;
            }
            // 크리티컬 히트 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage * 1.6), RawMaxDamage * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage, (RawMaxDamage + 1));
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            return info;
        }

        public override WeaponSkill Clone()
        {
            return new WeaponSkill(Name, Weapon, Stat, Multiplier);
        }
    }

    

    public class MagicSkill : DamageSkill
    {
        public int Level { get; private set; }
        public int ManaCost { get; private set; }
        public Stat Conjurer { get; set; }
        /// <summary>
        /// 마법 스킬의 생성자
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="min">최소 대미지</param>
        /// <param name="max">최대 대미지</param>
        /// <param name="crit">크리티컬 확률</param>
        /// <param name="acc">명중률</param>
        /// <param name="type">대미지 타입</param>
        /// <param name="conjurer">스킬을 사용하는 크리쳐</param>
        public MagicSkill(string name, int min, int max, double crit, double acc, 
            DamageType type, int level, int manacost, Stat conjurer)
            : base(name, min, max, crit, acc, type)
        {
            Conjurer = conjurer;
            Level = level;
            ManaCost = manacost;
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
                    info = new AttackInfo(true, false, 0, DamageType);
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType);
                    return info;
            }
            // 크리티컬 히트 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage * 1.6), RawMaxDamage * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage, (RawMaxDamage + 1));
                    info.IsCritical = false;
                    info.Damage = damage;
                    break;
            }
            return info;
        }

        public override MagicSkill Clone()
        {
            return new MagicSkill(Name, RawMinDamage, RawMaxDamage, 
                CriticalChance, Accuracy, DamageType, Level, ManaCost, Conjurer);
        }
    }

    public class EffectSkill : DamageSkill
    {
        public double EffectChance { get; set; }
        public Effect GiveEffect { get; set; }
        /// <summary>
        /// 버프/디버프를 동반하는 스킬의 생성자
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="min">최소 대미지</param>
        /// <param name="max">최대 대미지</param>
        /// <param name="crit">크리티컬 확률</param>
        /// <param name="acc">명중률</param>
        /// <param name="effchance">버프/디버프 확률</param>
        /// <param name="type">대미지 타입</param>
        /// <param name="give">주려는 효과(반드시 new로 생성해서 넣을 것)</param>
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
                    info = new AttackInfo(true, false, 0, DamageType);
                    break;
                default:
                    info = new AttackInfo(false, false, 0, DamageType);
                    return info;
            }
            // 크리티컬 여부 검사
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage * 1.6), RawMaxDamage * 2 + 1);
                    info.IsCritical = true;
                    info.Damage = damage;
                    break;

                default:
                    damage = r.Next(RawMinDamage, (RawMaxDamage + 1));
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

        public override EffectSkill Clone()
        {
            return new EffectSkill(Name, RawMinDamage, RawMaxDamage, 
                CriticalChance, Accuracy, 
                EffectChance, DamageType, GiveEffect);
        }
    }

    public class HealSkill : Skill
    {
        public HealSkill(string name) : base(name)
        {
        }
    }
}
