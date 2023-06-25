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
        public bool IsHitted { get; set; }
        public bool IsCritical { get; set; }
        public int Damage { get; set; }
        public DamageType DamageType { get; set; }
        public Effect? Effect { get; set; }
        public AttackInfo(bool ishitted, bool isCritical, int damage, DamageType dType, Effect? effect = null)
        {
            IsHitted = ishitted;
            IsCritical = isCritical;
            Damage = damage;
            DamageType = dType;
            Effect = effect;
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
}

