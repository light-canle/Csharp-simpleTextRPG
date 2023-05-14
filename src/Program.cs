using Combat;
using VariousEntity;

namespace C__RPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Creature e1 = new Creature("aa", 20, 10, 6, 6, 6, 0, 0);
            Creature e2 = new Creature("bb", 25, 10, 5, 6, 7, 1, 0);
            Skill s1 = new Skill("test", 2, 5, 0.1, 1);
            e1.Abilities.Add(s1);
            e1.Attack(ref e2, e1.Abilities[0]);
            Console.WriteLine($"{e1.Stat.HP} {e2.Stat.HP}");
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