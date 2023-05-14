using Combat;
using VariousItem;

namespace VariousEntity
{
    public class Entity : ICloneable
    {
        public string? Name { get; protected set; }
        public Entity(string? name) {
            Name = name;
        }
        public virtual object Clone() { 
            return new Entity(Name);
        }
    }
    public class Creature : Entity
    {
        protected int hp;
        protected int mp;
        public int HP
        {
            get { return hp; }
            protected set
            {
                if (value >= MaxHP)
                {
                    hp = MaxHP;
                }
                else if (value <= 0)
                {
                    hp = 0;
                }
                else
                {
                    hp = value;
                }
            }
        }
        public int MP
        {
            get { return mp; }
            protected set
            {
                if (value >= MaxMP)
                {
                    mp = MaxMP;
                }
                else if (value <= 0)
                {
                    mp = 0;
                }
                else
                {
                    mp = value;
                }
            }
        }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int Strength { get; protected set; }
        public int Agility { get; protected set; }
        public int Spell { get; protected set; }
        public int AC { get; protected set; }
        public int MR { get; protected set; }
        public List<Effect> Effects { get; protected set; }
        public List<Skill> Abilities { get; protected set; }
        public Resistance Resistance { get; protected set; }

        // ====================생성자====================
        public Creature(string name) : base(name)
        {
            MaxHP = 20;
            HP = MaxHP;
            MaxMP = 6;
            MP = MaxMP;
            Strength = 5;
            Agility = 5;
            Spell = 5;
            AC = 0;
            MR = 0;
            Effects = new List<Effect>();
            Abilities = new List<Skill>();
            Resistance = new Resistance();
        }

        public Creature(string name, int hp, int mp, int strength, int agility, int spell, int ac, int mr) : base(name)
        {
            MaxHP = hp;
            HP = MaxHP;
            MaxMP = mp;
            MP = MaxMP;
            Strength = strength;
            Agility = agility;
            Spell = spell;
            AC = ac;
            MR = mr;
            Effects = new List<Effect>();
            Abilities = new List<Skill>();
            Resistance = new Resistance();
        }

        // ====================메소드====================
        /// <summary>
        /// 인자로 받은 다른 엔티티를 공격한다.
        /// </summary>
        public virtual void Attack(ref Creature c, Skill skill)
        {
            switch (skill.Attack)
            {
                case AttackType.Normal:
                    c.ApplyDamage(skill.Damage());
                    break;
            }
        }

        /// <summary>
        /// Attack() 함수 내부에서 쓰는 메소드
        /// 상대가 반환한 공격의 대미지를 방어력에 따라 줄인 뒤 대미지를 적용한다.
        /// </summary>
        protected virtual void ApplyDamage(AttackInfo attackInfo)
        {
            Random rand = new Random();
            int final_damage = attackInfo.Damage;
            // AC, MR에 따라 대미지를 줄인다.
            switch (attackInfo.DamageType)
            {
                case DamageType.Normal:
                    final_damage -= rand.Next(AC / 2, AC + 1);
                    break;
                default:
                    final_damage -= rand.Next(MR / 3, MR + 1);
                    break;
            }
            // 만약 공격 타입이 원소 공격이고, 적절한 원소 저항이 있는 경우
            // 대미지를 저항% 만큼 추가로 줄인다.
            switch (attackInfo.DamageType)
            {
                case DamageType.Fire:
                    final_damage = (int)((final_damage) * (double)((100 - Resistance.Fire) / 100.0));
                    break;
                case DamageType.Ice:
                    final_damage = (int)((final_damage) * (double)((100 - Resistance.Ice) / 100.0));
                    break;
                case DamageType.Electric:
                    final_damage = (int)((final_damage) * (double)((100 - Resistance.Electric) / 100.0));
                    break;
            }
            // 최종 대미지만큼 체력을 감소시킨다.
            HP = HP - final_damage;
        }


        /// <summary>
        /// 엔티티에게 걸려있는 효과들을 적용하는 함수
        /// </summary>
        public virtual void ApplyEffect()
        {
            Random rand = new Random();
            if (Effects.Count == 0)
            {
                return;
            }
            // 순차적으로 루프를 돌며 각각의 효과에 대한 상태 변화를 적용함
            for (int i = 0; i < Effects.Count; i++)
            {
                switch (Effects[i].Type)
                {
                    // 화상
                    case EffectType.Burn:
                        // 불 저항 O : 저항% 만큼 대미지 감소
                        // 불 저항 X : 체력의 Lv * 2 ~ Lv * 4% 만큼 피해를 입힘
                        HP -= (int)rand.NextDouble();
                        break;

                }
                Effects[i].Duration -= 1;
            }
            // 여기서 Duration이 0이하가 된 효과들을 제거
            Predicate<Effect> predicate = (e) =>
            {
                return e.Duration <= 0;
            };
            Effects.RemoveAll(predicate);
        }

        /// <summary>
        /// 효과를 엔티티에게 추가함
        /// </summary>
        public virtual void AddEffect(Effect e)
        {
            if (Effects.Count == 0)
            {
                Effects.Add(e);
                return;
            }
            // 해당 효과가 이미 있는지 확인
            for (int i = 0; i < Effects.Count; i++)
            {
                // 있는 경우
                /*
                강도 차이
                0 => 남은 턴 수에 e의 턴 수를 그대로 더함
                N => 강도가 더 큰 것으로 교체
                이미 있던 것의 강도가 더 큼 : 턴 수를 ((새로운 효과의 턴수) div (N+1)) 만큼 추가
                새로 들어오는 것의 강도가 더 큼 : 턴 수를 floor((이미 있던 턴 수 div N) + 새로운 효과의 턴 수 / 2)로 지정
                */
                if (Effects[i].Type == e.Type)
                {
                    if (Effects[i].Strength == e.Strength)
                    {
                        Effects[i].Duration += e.Duration;
                    }
                    else if (Effects[i].Strength > e.Strength)
                    {
                        int diff = Effects[i].Strength - e.Strength;
                        Effects[i].Duration += (e.Duration) / diff;
                    }
                    else
                    {
                        int diff = e.Strength - Effects[i].Strength;
                        Effects[i].Strength = e.Strength;
                        Effects[i].Duration = (Effects[i].Duration / diff + e.Duration) / 2;
                    }
                    return;
                }
            }
            Effects.Add(e);
        }

        /// <summary>
        /// 엔티티에게 아이템을 사용함
        /// </summary>

        /// <summary>
        /// 엔티티가 인자로 받은 행동을 함
        /// </summary>
    }

    public class ArmedEntity : Entity
    {
        public Weapon? EquippedWeapon { get; private set; }
        public Armor?[] EquippedArmors { get; }
        public Accessory?[] EquippedAccessories { get; set; }
        public ArmedEntity(string name) : base(name)
        {
            EquippedWeapon = null;
            EquippedArmors = new Armor[3];
            EquippedAccessories = new Accessory[4];
        }

        /// <summary>
        /// 무기/아이템을 장착한다.
        /// </summary>
        public void Equip<T>(T obj) where T : Item
        {
            switch (obj)
            {
                case Weapon w:
                    if (EquippedWeapon != null)
                    {
                        UnEquip<Weapon>(Position.Weapon);
                    }
                    break;
                case Armor a:

                    break;
                case Accessory a:
                    break;
            }
        }

        /// <summary>
        /// 무기/아이템의 장착을 해제한다.
        /// </summary>
        public Equipable? UnEquip<T>(Position pos) where T : Equipable
        {
            switch (pos)
            {
                case Position.Weapon:
                    Weapon? w = new Weapon();
                    w = EquippedWeapon?.Clone() ?? null;
                    return w;
                default:
                    throw new ArgumentException("UnEquip : Position enum을 이용해 적절한 인자를 넣어주세요.");
            }
        }
    }
}