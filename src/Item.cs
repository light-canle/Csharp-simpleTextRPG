using Combat;
using VariousEntity;

// TODO : 아이템들을 위한 Clone 메소드

namespace VariousItem
{

    // 무기나 방어구에 부여할 수 있는 마법의 종류
    public enum EnchantmentType
    {
        AttackReinforcement, // 대미지 증가
        Sharpness, // 치명타 확률 증가
        Heavyness, // 치명타 위력 증가
        Accuracy, // 정확도 증가
        Hardness, // 방어력(AC) 증가
        AntiMagic, // 마법 저항(MR) 증가

    }

    // 무기나 방어구의 품질
    public enum Quality
    {
        Broken,
        Weakness,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    // 무기를 장착하는 위치
    public enum Position
    {
        Weapon,
        HeadArmor,
        TopArmor,
        BottomArmor,
        Accessory,
    }

    // 스크롤의 효과 종류
    public enum ScrollType
    {
        Magic,
        Enchantment,
        Sleep,
    }

    // 반지 종류
    public enum RingType
    {
        BaseStatUp,
        ResistanceUp,
        GiveSpecialAbility,
    }

    // 인챈트 클래스
    public sealed class Enchantment : ICloneable
    {
        public EnchantmentType Type { get; private set; }
        public int Level { get; private set; }

        public Enchantment(EnchantmentType type)
        {
            Type = type;
        }

        public Enchantment(EnchantmentType type, int level)
        {
            Type = type;
            Level = level;
        }

        public object Clone()
        {
            return new Enchantment(Type, Level);
        }
    }

    public class Item : ICloneable
    {
        public string Name { get; protected set; }
        public int Cost { get; protected set; }

        // ====================생성자====================
        public Item(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public virtual object Clone()
        {
            Item i = new Item("", 0);
            i.Name = this.Name;
            i.Cost = this.Cost;
            return i;
        }
    }

    public class Equipable : Item
    {
        public int Reinforcement { get; set; }
        public Quality Quality { get; set; }
        public Position Position { get; set; }
        public List<Enchantment> EnchantList { get; set; }

        public Equipable(string name, int cost, Position pos, int reinforcement = 0, Quality quality = Quality.Common) : base(name, cost)
        {
            Position = pos;
            Reinforcement = reinforcement;
            Quality = quality;
            EnchantList = new List<Enchantment>();
        }

        public override Equipable Clone()
        {
            Equipable e = new Equipable("", 0, Position.Weapon);
            e.Name = this.Name;
            e.Cost = this.Cost;
            e.Position = this.Position;
            e.Reinforcement = this.Reinforcement;
            e.Quality = this.Quality;
            e.EnchantList = this.EnchantList.ConvertAll(en => new Enchantment(en.Type, en.Level));
            return e;
        }
        /// <summary>
        /// 인자로 받은 인챈트 추가
        /// (!중요) 장착 가능한 무기의 종류(무기, 방어구, 장신구)에 따라
        /// 할 수 있는 인챈트들의 종류가 다르므로, 자식 클래스에서는
        /// 반드시 이 메소드를 오버라이드 해서 구현할 것
        /// </summary>
        /// <param name="enchant">추가할 인챈트</param>
        public virtual void Enchant(Enchantment enchant)
        {
            EnchantList.Add(enchant);
        }

        public virtual void UpdateEnchant() { }
    }

    public class Weapon : Equipable
    {
        public int RawMinDamage { get; set; }
        public int RawMaxDamage { get; set; }
        public DamageType DamageType { get; set; }
        public double CriticalChance { get; set; }
        public double Accuracy { get; set; }


        // ====================생성자====================
        public Weapon() : base("Air", 0, Position.Weapon)
        {
            RawMaxDamage = 0;
            RawMaxDamage = 0;
            CriticalChance = 0;
            Accuracy = 0;
        }
        public Weapon(string name, int cost, int min, int max, int critical, int accuracy, Quality quality) : base(name, cost, Position.Weapon, 0, quality)
        {
            RawMaxDamage = min;
            RawMaxDamage = max;
            CriticalChance = critical;
            Accuracy = accuracy;
        }
        // ====================메소드====================
        /// <summary>
        /// 이 무기의 기본 대미지를 반환한다.
        /// </summary>
        public AttackInfo Attack()
        {
            Random rand = new Random();
            int minDamage = RawMinDamage + Reinforcement;
            int maxDamage = RawMaxDamage + 2 * Reinforcement;

            int damage = 0;
            switch (rand.NextDouble())
            {
                // 치명타인 경우
                case double d when d <= CriticalChance:
                    damage = rand.Next((int)Math.Floor((double)maxDamage * 1.6), (int)Math.Floor((double)maxDamage * 2));
                    return new AttackInfo(true, damage, DamageType);
                // 일반 공격인 경우
                default:
                    damage = rand.Next(minDamage, maxDamage);
                    return new AttackInfo(false, damage, DamageType);
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

            w.Reinforcement = Reinforcement;
            w.Quality = Quality;
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
                    throw new ArgumentException("Weapon.Enchant : 무기에 할 수 없는 인챈트입니다.");
            }
        }
    }

    public class Armor : Equipable
    {
        public int AC { get; set; }
        public int MR { get; set; }
        public Resistance Resistance { get; set; }

        // ====================생성자====================
        public Armor(string name, int cost, int ac, int mr, Position pos, Quality quality) : base(name, cost, pos, 0, quality)
        {
            AC = ac;
            MR = mr;
            Resistance = new Resistance();
        }

        public Armor(string name, int cost, int ac, int mr, Position pos, int reinforcement, Quality quality, Resistance r) : base(name, cost, pos, reinforcement, quality)
        {
            AC = ac;
            MR = mr;
            Resistance = (Resistance)r.Clone();
        }

        // ====================메소드====================
        public override Armor Clone()
        {
            return new Armor(Name, Cost, AC, MR, Position, Quality);
        }
    }

    public class Accessory : Equipable
    {
        public RingType Type { get; private set; }
        public Stat? ChangeStats { get; private set; }
        public Resistance? Resistance { get; private set; }
        public Accessory(string name, int cost, RingType type, Stat? stat = null,
            Resistance? resistance = null) : base(name, cost, Position.Accessory, 0, Quality.Common)
        {
            
            Position = Position.Accessory;
            Type = type;
            switch(type)
            {
                case RingType.BaseStatUp:
                    ChangeStats = (Stat?)stat?.Clone() 
                        ?? throw new ArgumentNullException("Accessory : RingType이 BaseStatUp이지만 stat인자가 null입니다."); ;
                    break;
                case RingType.ResistanceUp:
                    Resistance = (Resistance?)resistance?.Clone() 
                        ?? throw new ArgumentNullException("Accessory : RingType이 ResistanceUp이지만 resistance인자가 null입니다.");
                    break;
                default:
                    throw new ArgumentException("Accessory : 잘못된 RingType입니다.");
            }
        }

        public override Accessory Clone()
        {
            return new Accessory(Name, Cost, Type, ChangeStats, Resistance);
        }
    }

    public class Consumable : Item
    {
        public Consumable(string name, int cost) : base(name, cost)
        {
        }
        public virtual void Consume(Creature e) { 
        
        }
    }

    public class Potion : Consumable
    {
        public Effect Effect { get; protected set; }

        public Potion(string name, int cost, Effect effect) : base(name, cost)
        {
            Effect = (Effect)effect.Clone();
        }

        /// <summary>
        /// 해당 포션을 사용한다.
        /// </summary>
        public override void Consume(Creature e)
        {
            e.AddEffect(Effect);
        }

        public override Potion Clone()
        {
            return new Potion(Name, Cost, Effect);
        }
    }

    public class Scroll : Consumable
    {
        public ScrollType Type { get; protected set; }
        public Skill? Skill { get; protected set; }
        public Scroll(string name, int cost, ScrollType type, Skill? skill = null) : base(name, cost)
        {
            Type = type;
            Skill = skill;
        }
        public override void Consume(Creature e)
        {
            switch (Type)
            {
                case ScrollType.Magic:
                    if (Skill == null)
                        throw new NullReferenceException("Scroll.Consume() : 스크롤의 타입이 Magic이지만, 대응되는 Skill이 없습니다.");
                    // 대응하는 Skill을 사용한다.

                    break;
                case ScrollType.Enchantment:
                    // TODO : 선택한 아이템 강화함
                    break;
            }
        }

        public override Scroll Clone()
        {
            return new Scroll(Name, Cost, Type, Skill);
        }
    }
}