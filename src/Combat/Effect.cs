using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using VariousEntity;

namespace Combat
{
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

        public void Apply(Creature creature, bool printLog = true)
        {
            Random rand = new Random();
            // 약화, 강화, 흐릿함은 여기가 아니라 skill 쪽에서 처리한다.
            switch (Type)
            {
                // 화상
                case EffectType.Burn:
                    // 불 저항 O : 저항% 만큼 대미지 감소
                    // 불 저항 X : 체력의 Lv * 2 ~ Lv * 4% 만큼 피해를 입힘
                    double fireDamage = creature.Stat.MaxHP * (rand.NextDouble() * (2 * Strength) + (2.0 * Strength)) / 100.0;
                    fireDamage *= (100 - creature.Resistance.Fire) / 100.0;
                    fireDamage = Math.Max((int)Math.Round(fireDamage, MidpointRounding.AwayFromZero),
                        (creature.Resistance.Fire == 100) ? 0 : 1);
                    if (printLog) TUI.ColorPrint(255, 128, 128, creature.Name + "은(는) 화상으로 인해 " + fireDamage + "의 피해를 입었다.");
                    creature.Stat.HP -= (int)fireDamage;
                    break;
                // 약화
                case EffectType.Weakness:
                    // Skip
                    break;
                // 시야 흐릿함
                case EffectType.Blurry:
                    // Skip
                    break;
                // 독
                case EffectType.Poison:
                    // 독 저항 O : 저항% 만큼 대미지 감소
                    // 독 저항 X : 체력의 Lv ~ Lv * 3 만큼 피해를 입힘
                    double posionDamage = rand.Next(Strength, Strength * 3);
                    posionDamage *= (100 - creature.Resistance.Poison) / 100.0;
                    posionDamage = Math.Round(posionDamage, MidpointRounding.AwayFromZero);
                    if (printLog) TUI.ColorPrint(255, 128, 128, creature.Name + "은(는) 독으로 인해 " + posionDamage + "의 피해를 입었다.");
                    creature.Stat.HP -= (int)posionDamage;
                    break;
                // 용해
                case EffectType.Dissolve:
                    break;
                // 재생
                case EffectType.Regeneration:
                    // Lv ~ Lv * 3만큼 회복
                    int healAmount = rand.Next(Strength, Strength * 3);
                    creature.Stat.HP += healAmount;
                    break;
                // 힘
                case EffectType.Strengthen: break;
                case EffectType.Transparency: break;
                case EffectType.Penetrate: break;
            }

        }

        public string Name()
        {
            switch (Type)
            {
                // 화상
                case EffectType.Burn: return "화상";
                // 약화
                case EffectType.Weakness: return "약화";
                // 시야 흐릿함
                case EffectType.Blurry: return "흐릿함";
                // 독
                case EffectType.Poison: return "독";
                // 용해
                case EffectType.Dissolve: return "용해";
                // 재생
                case EffectType.Regeneration: return "재생";
                // 힘
                case EffectType.Strengthen: return "힘";
                case EffectType.Transparency: return "투명화";
                case EffectType.Penetrate: return "투시";
                default: return "";
            }
        }
    }
}
