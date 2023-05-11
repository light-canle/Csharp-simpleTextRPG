using VariousEntity;
using VariousItem;

namespace Combat{
    // 포션이나 공격이 줄 수 있는 효과
    public enum EffectType{
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
    public enum DamageType{
        Normal, // 일반 물리 공격
        Energy,
        Fire,
        Ice,
        Electric,
        Water,

    }

    // 스킬에서 공격의 방식
    public enum AttackType{
        Normal, // 일반 공격
        Magic, // 마법 공격
        Weapon, // 무기 사용
        Summon, // 소환술
        Effect, // 버프/디버프/회복
        Speical, // 특수 공격
    }



    // ================================Class - 속성================================
    public sealed class Resistance{
        public int Fire { get; private set; }
        public int Electric { get; private set; }
        public int Ice { get; private set; }
        public int Poison { get; private set; }
        public int Acid { get; private set; }
        // ====================생성자====================
        public Resistance(){
            Fire = 0;
            Electric = 0;
            Ice = 0;
            Poison = 0;
            Acid = 0;
        }
        public Resistance(int fire, int electric, int ice, int poison, int acid){
            Fire = fire;
            Electric = electric;
            Ice = ice;
            Poison = poison;
            Acid = acid;
        }
        // ====================메소드====================
        public Resistance DeepCopy() {
            Resistance r = new Resistance();
            r.Fire = Fire;
            r.Electric = Electric;
            r.Ice = Ice;
            r.Poison = Poison;
            r.Acid = Acid;
            return r;
        }
    }

    public sealed class AttackInfo{
        public AttackInfo(bool isCritical, int damage, DamageType dType){
            IsCritical = isCritical;
            Damage = damage;
            DamageType = dType;
        }
        public bool IsCritical { get; }
        public int Damage{ get; }
        public DamageType DamageType{ get; }
    }

    public sealed class Effect{
        public EffectType Type { get; private set; }
        public int Strength { get; set; }
        public int Duration { get; set; }
        // ====================생성자====================
        public Effect(EffectType type, int strength, int duration){
            Type = type;
            Strength = strength;
            Duration = duration;
        }
        // ====================메소드====================
        public Effect DeepCopy() {
            Effect e = new Effect(EffectType.Blurry, 0, 0);
            e.Type = Type;
            e.Strength = Strength;
            e.Duration = Duration;
            return e;
        }
    }

    public sealed class Skill{
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
        public Skill (string name, int min, int max, double crit, double acc, 
        AttackType attack = AttackType.Normal, DamageType type = Combat.DamageType.Normal){
            Name = name;
            Attack = attack;
            RawMinDamage = min;
            RawMaxDamage = max;
            CriticalChance = crit;
            Accuracy = acc;
            DamageType = type;
        }
        // 버프 / 디버프 스킬 생성자
        public Skill(string name, double acc, double effchance, Effect give, AttackType attack = AttackType.Effect){
            Name = name;
            Attack = attack;
            Accuracy = acc;
            EffectChance = effchance;
            GiveEffect = give;
        }
        public Skill(string name, AttackType attack, int min, int max, double crit, double acc, DamageType type){
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
        public AttackInfo Damage() { 
            Random r = new Random();
            int damage;
            switch (r.NextDouble()){
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

