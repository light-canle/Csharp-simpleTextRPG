using Combat;
using System;
using VariousEntity;

namespace VariousItem
{
    // TODO : Accessory 종류 분화
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
            switch (type)
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

}
