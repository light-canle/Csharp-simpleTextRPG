using Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;

namespace VariousItem
{
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
