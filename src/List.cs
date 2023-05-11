using Combat;

namespace List
{
    public static class SkillList
    {
        public static Dictionary<string, Skill> Skills { get; }

        static SkillList()
        {
            Skills = new Dictionary<string, Skill>();
            Skills.Add("일반 공격", new Combat.Skill("일반 공격", 1, 10, 0.03, 0.95));
            Skills.Add("속공", new Combat.Skill("속공", 1, 8, 0.06, 0.90));
            Skills.Add("강공", new Combat.Skill("강공", 2, 12, 0.03, 0.90));
        }
    }
}