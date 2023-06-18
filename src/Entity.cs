using Combat;
using VariousItem;

namespace VariousEntity
{
    public class Stat : ICloneable
    {
        public int hp;
        public int mp;
        public int HP
        {
            get { return hp; }
            set
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
            set
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
        public int BaseMaxHP { get; set; }
        public int BaseMaxMP { get; set; }
        public int BaseStrength { get; set; }
        public int BaseAgility { get; set; }
        public int BaseSpell { get; set; }
        public int BaseAC { get; set; }
        public int BaseMR { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Spell { get; set; }
        public int AC { get; set; }
        public int MR { get; set; }
        public Stat(int hp = 20, int mp = 6, int strength = 5, int agility = 5, int spell = 5, int ac = 0, int mr = 0)
        {
            BaseMaxHP = hp;
            MaxHP = BaseMaxHP;
            HP = MaxHP;

            BaseMaxMP = mp;
            MaxMP = BaseMaxMP;
            MP = MaxMP;

            BaseStrength = strength;
            BaseAgility = agility;
            BaseSpell = spell;
            BaseAC = ac;
            BaseMR = mr;

            Strength = BaseStrength;
            Agility = BaseAgility;
            Spell = BaseSpell;
            AC = BaseAC;
            MR = BaseMR;
        }

        public object Clone()
        {
            return new Stat(hp : BaseMaxHP, mp : BaseMaxMP, 
                strength: BaseStrength, agility: BaseAgility, 
                spell: BaseSpell, ac: BaseAC, mr: BaseMR);
        }
    }
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
        public Stat Stat { get; protected set; }
        public List<Effect> Effects { get; protected set; }
        public List<Skill> Abilities { get; protected set; }
        public Resistance Resistance { get; protected set; }

        // ====================생성자====================
        public Creature(string name) : base(name)
        {
            Stat = new Stat();
            Effects = new List<Effect>();
            Abilities = new List<Skill>();
            Resistance = new Resistance();
        }

        public Creature(string name, int hp, int mp, int strength, int agility, int spell, int ac, int mr) : base(name)
        {
            Stat = new Stat(hp : hp, mp : mp, strength : strength,
                agility : agility, spell : spell, ac : ac, mr : mr);
            Effects = new List<Effect>();
            Abilities = new List<Skill>();
            Resistance = new Resistance();
        }

        // ====================메소드====================
        /// <summary>
        /// 인자로 받은 다른 크리쳐를 공격한다.
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
                    final_damage -= rand.Next(Stat.AC / 2, Stat.AC + 1);
                    break;
                default:
                    final_damage -= rand.Next(Stat.MR / 3, Stat.MR + 1);
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
            Stat.HP = Stat.HP - final_damage;
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
                        Stat.HP -= (int)rand.NextDouble();
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

    public class ArmedEntity : Creature
    {
        public Weapon? EquippedWeapon { get; private set; }
        public Armor?[] EquippedArmors { get; }
        public List<Accessory> EquippedAccessories { get; set; }
        public ArmedEntity(string name) : base(name)
        {
            EquippedWeapon = null;
            EquippedArmors = new Armor[3];
            EquippedAccessories = new List<Accessory>();
        }

        /// <summary>
        /// 무기/아이템을 장착한다.
        /// </summary>
        /// <typeparam name="T">장착 가능한 아이템만 인자로 넣을 수 있다.</typeparam>
        /// <param name="obj">장착 하려는 장비</param>
        public void Equip<T>(T obj) where T : Equipable
        {
            switch (obj)
            {
                case Weapon w:
                    if (EquippedWeapon != null)
                    {
                        UnEquip<Weapon>(Position.Weapon);
                    }
                    StatUpdate();
                    break;
                case Armor a:
                    switch (a.Position)
                    {
                        case Position.HeadArmor:
                            if (EquippedArmors[0] != null) UnEquip<Armor>(Position.HeadArmor);
                            break;
                        case Position.TopArmor:
                            if (EquippedArmors[1] != null) UnEquip<Armor>(Position.TopArmor);
                            break;
                        case Position.BottomArmor:
                            if (EquippedArmors[2] != null) UnEquip<Armor>(Position.BottomArmor);
                            break;
                    }
                    StatUpdate();
                    break;
                case Accessory a:
                    if (EquippedAccessories.Count < 4)
                    {
                        EquippedAccessories.Append(a);
                    }
                    StatUpdate();
                    break;
            }
        }

        /// <summary>
        /// 무기/아이템의 장착을 해제한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pos">무기/방어구/장신구 중 어느 것을 해제할 것인지 여부</param>
        /// <param name="accPos">(장신구인 경우) 위치(인덱스)</param>
        /// <returns>장착 해제한 장비를 반환</returns>
        /// <exception cref="Exception">해제하려는 장비가 null인 경우 예외 반환</exception>
        /// <exception cref="ArgumentException">올바르지 않은 Position인 경우 반환</exception>
        public Equipable? UnEquip<T>(Position pos, int accPos = 0) where T : Equipable
        {
            Armor? a;
            Weapon? w;
            Accessory? ac;
            switch (pos)
            {
                case Position.Weapon:
                    w = EquippedWeapon?.Clone() ?? throw new Exception("UnEquip : 장착 해제할 무기가 없습니다.");
                    EquippedWeapon = null;
                    StatUpdate();
                    return w;
                case Position.HeadArmor:
                    a = EquippedArmors?[0]?.Clone() ?? throw new Exception("UnEquip : 장착 해제할 방어구가 없습니다.");
                    EquippedArmors[0] = null;
                    StatUpdate();
                    return a;
                case Position.TopArmor:
                    a = EquippedArmors?[1]?.Clone() ?? throw new Exception("UnEquip : 장착 해제할 방어구가 없습니다.");
                    EquippedArmors[1] = null;
                    StatUpdate();
                    return a;
                case Position.BottomArmor:
                    a = EquippedArmors?[2]?.Clone() ?? throw new Exception("UnEquip : 장착 해제할 방어구가 없습니다.");
                    EquippedArmors[2] = null;
                    StatUpdate();
                    return a;
                case Position.Accessory:
                    ac = EquippedAccessories[accPos].Clone() ?? throw new Exception("UnEquip : 장착 해제할 장신구가 없습니다.");
                    EquippedAccessories.RemoveAt(accPos);
                    StatUpdate();
                    return ac;
                default:
                    throw new ArgumentException("UnEquip : Position enum을 이용해 적절한 인자를 넣어주세요.");
            }
        }

        /// <summary>
        /// 새 장비를 해제/장착 했을 때 사용 - 장비한 아이템의 능력치 변화를 반영한다.
        /// </summary>
        public void StatUpdate()
        {
            // 방어력, 속성 저항
            Stat.AC = Stat.BaseAC;
            Stat.MR = Stat.BaseMR;

            Resistance.Fire = Resistance.BaseFire;
            Resistance.Electric = Resistance.BaseElectric;
            Resistance.Ice = Resistance.BaseIce;
            Resistance.Poison = Resistance.BasePoison;
            Resistance.Acid = Resistance.BaseAcid;

            for (int i = 0; i < EquippedArmors.Length; i++)
            {
                Stat.AC += EquippedArmors[i]?.AC ?? 0;
                Stat.MR += EquippedArmors[i]?.MR ?? 0;

                Resistance.Fire += EquippedArmors[i]?.Resistance.Fire ?? 0;
                Resistance.Electric += EquippedArmors[i]?.Resistance.Electric ?? 0;
                Resistance.Ice += EquippedArmors[i]?.Resistance.Ice ?? 0;
                Resistance.Poison += EquippedArmors[i]?.Resistance.Poison ?? 0;
                Resistance.Acid += EquippedArmors[i]?.Resistance.Acid ?? 0;
            }

            for (int i = 0; i < EquippedAccessories.Count; i++)
            {
                Stat.AC += EquippedAccessories[i].ChangeStats?.AC ?? 0;
                Stat.MR += EquippedAccessories[i].ChangeStats?.MR ?? 0;

                Resistance.Fire += EquippedAccessories[i].Resistance?.Fire ?? 0;
                Resistance.Electric += EquippedAccessories[i].Resistance?.Electric ?? 0;
                Resistance.Ice += EquippedAccessories[i].Resistance?.Ice ?? 0;
                Resistance.Poison += EquippedAccessories[i].Resistance?.Poison ?? 0;
                Resistance.Acid += EquippedAccessories[i].Resistance?.Acid ?? 0;
            }
        }
    }
}