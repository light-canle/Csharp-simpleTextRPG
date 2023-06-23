namespace Combat
{
    // 포션이나 공격이 줄 수 있는 효과
    public enum EffectType
    {
        // 디버프
        Burn,   // 연소
        Paralysis,  // 마비
        Freezing,   // 빙결
        Wet, // 젖음
        Weakness, // 약화
        Blurry, // 흐릿함(명중률 감소)
        Poison, // 독
        Dissolve, // 용해(산에 의해 녹음)

        // 버프
        Regeneration, // 재생
        Strengthen, // 강화
        Transparency, // 투명화
        Penetrate, // 투시

    }
    // 공격의 종류
    public enum DamageType
    {
        Normal, // 일반 물리 공격
        Energy,
        Fire,
        Ice,
        Electric,
        Water,

    }

    // 스킬에서 공격의 방식
    public enum AttackType
    {
        Normal, // 일반 공격
        Magic, // 마법 공격
        Weapon, // 무기 사용
        Summon, // 소환술
        Effect, // 버프/디버프/회복
        Speical, // 특수 공격
    }

    // ================================Class - 속성================================
    public sealed class Resistance : ICloneable
    {
        public int BaseFire { get; set; }
        public int BaseElectric { get; set; }
        public int BaseIce { get; set; }
        public int BasePoison { get; set; }
        public int BaseAcid { get; set; }
        public int Fire { get; set; }
        public int Electric { get; set; }
        public int Ice { get; set; }
        public int Poison { get; set; }
        public int Acid { get; set; }
        // ====================생성자====================

        public Resistance(int fire = 0, int electric = 0, int ice = 0, int poison = 0, int acid = 0)
        {
            BaseFire = fire;
            BaseElectric = electric;
            BaseIce = ice;
            BasePoison = poison;
            BaseAcid = acid;

            Fire = BaseFire;
            Electric = BaseElectric;
            Ice = BaseIce;
            Poison = BasePoison;
            Acid = BaseAcid;
        }
        // ====================메소드====================
        public object Clone()
        {
            return new Resistance(fire: BaseFire, electric: BaseElectric,
                ice: BaseIce, poison: BasePoison, acid: BaseAcid);
        }
    }

    public sealed class AttackInfo
    {
        public bool IsCritical { get; }
        public int Damage { get; }
        public DamageType DamageType { get; }
        public AttackInfo(bool isCritical, int damage, DamageType dType)
        {
            IsCritical = isCritical;
            Damage = damage;
            DamageType = dType;
        }
    }

    public sealed class Effect : ICloneable
    {
        public EffectType Type { get; private set; }
        public int Strength { get; set; }
        public int Duration { get; set; }
        // ====================생성자====================
        public Effect(EffectType type = EffectType.Blurry, int strength = 1, int duration = 0)
        {
            Type = type;
            Strength = strength;
            Duration = duration;
        }
        // ====================메소드====================
        public object Clone()
        {
            return new Effect(type: Type, strength: Strength, duration: Duration);
        }
    }

    public sealed class Skill
    {
        public string Name { get; private set; }
        public AttackType Attack { get; private set; }
        public int? RawMinDamage { get; set; }
        public int? RawMaxDamage { get; set; }
        public double? CriticalChance { get; set; }
        public double? Accuracy { get; set; }
        public DamageType? DamageType { get; set; }
        public double? EffectChance { get; set; }
        public Effect? GiveEffect { get; set; }
        // ====================생성자====================
        // 일반 공격 생성자 - 효과를 주지 않는 일반적인 대미지 공격
        public Skill(string name, int min, int max, double crit, double acc,
        AttackType attack = AttackType.Normal, DamageType type = Combat.DamageType.Normal)
        {
            Name = name;
            Attack = attack;
            RawMinDamage = min;
            RawMaxDamage = max;
            CriticalChance = crit;
            Accuracy = acc;
            DamageType = type;
        }
        // 버프 / 디버프 스킬 생성자
        public Skill(string name, double acc, double effchance, Effect give, AttackType attack = AttackType.Effect)
        {
            Name = name;
            Attack = attack;
            Accuracy = acc;
            EffectChance = effchance;
            GiveEffect = give;
        }
        public Skill(string name, AttackType attack, int min, int max, double crit, double acc, DamageType type)
        {
            Name = name;
            Attack = attack;
            RawMinDamage = min;
            RawMaxDamage = max;
            CriticalChance = crit;
            Accuracy = acc;
            DamageType = type;
        }
        // ====================메소드====================

        /// <summary>
        /// 이 스킬의 대미지를 리턴한다.
        /// AttackType이 Normal, Magic, Weapon, Special(대미지가 있음)일 때만 사용한다.
        /// </summary>
        public AttackInfo Damage()
        {
            Random r = new Random();
            int damage;
            switch (r.NextDouble())
            {
                case double d when d <= CriticalChance:
                    damage = r.Next((int)(RawMaxDamage.GetValueOrDefault() * 1.6), RawMaxDamage.GetValueOrDefault() * 2 + 1);
                    return new AttackInfo(true, damage, DamageType.GetValueOrDefault());

                default:
                    damage = r.Next(RawMinDamage.GetValueOrDefault(), (RawMaxDamage + 1).GetValueOrDefault());
                    return new AttackInfo(false, damage, DamageType.GetValueOrDefault());
            }
        }
    }
}

