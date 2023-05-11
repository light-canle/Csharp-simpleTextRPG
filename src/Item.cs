using Combat;
using VariousEntity;

// TODO : 아이템들을 위한 DeepCopy 메소드

namespace VariousItem
{

    // 무기나 방어구에 부여할 수 있는 마법의 종류
    public enum EnchantmentType
    {
        AttackReinforcement, // 대미지 증가
        Sharpness, // 치명타 확률 증가
        Heavyness, // 치명타 위력 증가
        Accuracy, // 정확도 증가
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
        FirstAccessory,
        SecondAccessory,
        ThirdAccessory,
        FourthAccessory,
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

    public sealed class Enchantment
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

        public Weapon DeepCopy()
        {
            Weapon w = new Weapon();
            w.RawMaxDamage = RawMaxDamage;
            w.RawMinDamage = RawMinDamage;
            w.DamageType = DamageType;
            w.CriticalChance = CriticalChance;
            w.Accuracy = Accuracy;
            w.Reinforcement = Reinforcement;
            w.Quality = Quality;
            w.EnchantList = EnchantList.ToList();
            return w;
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
            Resistance = r.DeepCopy();
        }
    }

    public class Accessory : Equipable
    {
        public RingType Type { get; private set; }
        public Accessory(string name, int cost, Position pos, RingType type) : base(name, cost, pos, 0, Quality.Common)
        {
            Type = type;
        }
    }

    public class Potion : Item
    {
        public Effect Effect { get; protected set; }

        public Potion(string name, int cost, Effect effect) : base(name, cost)
        {
            Effect = effect.DeepCopy();
        }

        /// <summary>
        /// 해당 포션을 사용한다.
        /// </summary>
        public void Consume(Entity e)
        {
            e.AddEffect(Effect);
        }
    }

    public class Scroll : Item
    {
        public ScrollType Type { get; protected set; }
        public Skill? Skill { get; protected set; }
        public Scroll(string name, int cost, ScrollType type, Skill? skill = null) : base(name, cost)
        {
            Type = type;
            Skill = skill;
        }
        public void Consume(Entity e)
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
    }
}