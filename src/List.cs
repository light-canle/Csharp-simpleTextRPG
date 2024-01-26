using Combat;

namespace List
{
    public static class SkillList
    {
        public static Dictionary<string, DamageSkill> Skills { get; }

        static SkillList()
        {
            Skills = new Dictionary<string, DamageSkill>
            {
                { "파이어볼 lv1", new Combat.DamageSkill("파이어볼 lv1", 3, 8, 0.05, 0.95, DamageType.Fire) },
                { "화염 방사 lv1", new Combat.DamageSkill("화염 방사 lv1", 4, 11, 0.04, 0.90, DamageType.Fire) },
                { "매직 미사일 lv1", new Combat.DamageSkill("매직 미사일 lv1", 2, 12, 0.03, 0.90, DamageType.Energy) }
            };
        }
    }
}