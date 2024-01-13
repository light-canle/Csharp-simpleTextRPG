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

        public void Apply(Equipable equipable)
        {
            switch (Type)
            {
                case EnchantmentType.AttackReinforcement:
                    switch (equipable)
                    {
                        case Weapon w:
                            w.MinDamage = (int)(w.RawMinDamage * (1.0 + Level * 0.1));
                            w.MaxDamage = (int)(w.RawMaxDamage * (1.0 + Level * 0.1));
                            break;
                        default: break;
                    }
                    break;
                case EnchantmentType.Sharpness:
                    switch (equipable)
                    {
                        case Weapon w:
                            w.CriticalChance += 0.01 * Level;
                            break;
                        default: break;
                    }
                    break;
                case EnchantmentType.Heavyness:
                    switch (equipable)
                    {
                        case Weapon w:
                            w.CriticalPower += 0.1 * Level;
                            break;
                        default: break;
                    }
                    break;
                case EnchantmentType.Accuracy:
                    switch (equipable)
                    {
                        case Weapon w:
                            w.Accuracy += 0.2 * Level;
                            break;
                        default: break;
                    }
                    break;
                case EnchantmentType.Hardness:
                    switch (equipable)
                    {
                        case Armor a:
                            a.Stat.AC += 1 * Level;
                            break;
                        default: break;
                    }
                    break;
                case EnchantmentType.AntiMagic:
                    switch (equipable)
                    {
                        case Armor a:
                            a.Stat.MR += 1 * Level;
                            break;
                        default: break;
                    }
                    break;
            }
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
            e.Name = Name;
            e.Cost = Cost;
            e.Position = Position;
            e.Reinforcement = Reinforcement;
            e.Quality = Quality;
            e.EnchantList = EnchantList.ConvertAll(en => new Enchantment(en.Type, en.Level));
            return e;
        }
        /// <summary>
        /// 인자로 받은 인챈트 추가
        /// (!중요) 장착 가능한 장비의 종류(무기, 방어구, 장신구)에 따라
        /// 할 수 있는 인챈트들의 종류가 다르므로, 자식 클래스에서는
        /// 반드시 이 메소드를 오버라이드 해서 구현할 것
        /// </summary>
        /// <param name="enchant">추가할 인챈트</param>
        public virtual void Enchant(Enchantment enchant)
        {
            EnchantList.Add(enchant);
        }

        /// <summary>
        /// 인챈트 리스트 안에 있는 인챈트들을 바탕으로 장비의 속성을
        /// 변경한다.
        /// </summary>
        public virtual void UpdateEnchant()
        {
            foreach (Enchantment e in EnchantList)
            {
                e.Apply(this);
            }
        }
    }
    
    //public class 

    public abstract class Consumable : Item
    {
        public Consumable(string name, int cost) : base(name, cost)
        {
            
        }
    }
}