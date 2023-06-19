using Combat;
using VariousEntity;
using Utils;

namespace C__RPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Creature e1 = new Creature("aa", 60, 25, 6, 7, 6, 2, 0);
            Creature e2 = new Creature("bb", 70, 20, 5, 6, 7, 3, 0);
            Skill s1 = new Skill("test", 3, 10, 0.1, 1);
            Skill s2 = new Skill("test2", 2, 6, 0.16, 1);
            e1.Abilities.Add(s1);
            e1.Abilities.Add(s2);
            e2.Abilities.Add(s1);
            e2.Abilities.Add(s2);

            Battle.Battle1v1(e1, e2);

            bool running = true;
            ConsoleKey press;
            while (running)
            {
                press = Console.ReadKey().Key;
                switch (press)
                {
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                }
            }
        }
    }
}