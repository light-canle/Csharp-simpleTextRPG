﻿using VariousEntity;
using Utils;
using Combat;
using Spectre.Console;

namespace C__RPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Creature e1 = new Creature("aa", 275, 25, 6, 7, 6, 5, 2, 0);
            Creature e2 = new Creature("bb", 270, 20, 5, 6, 7, 5, 3, 0);
            DamageSkill s1 = new DamageSkill("몸통박치기", 10, 15);
            DamageSkill s2 = new DamageSkill("기습", 3, 12, 0.33, 0.6);
            e1.Abilities.Add(s1);
            e1.Abilities.Add(s2);
            e2.Abilities.Add(s1);
            e2.Abilities.Add(s2);

            Battle.Battle1v1(e1, e2);

            var rate = Battle.WinRate(e1, e2, 10000);

            Console.WriteLine($"{e1.Name}의 승리 횟수 : {rate.Item1}");
            Console.WriteLine($"{e2.Name}의 승리 횟수 : {rate.Item2}");
            Console.WriteLine($"무승부 횟수 : {rate.Item3}");

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

            /*Creature smile = new Creature(name:"슬라임", hp : 25, mp : 4, 
                6, 5, 5, 5, 2, 0);
            Player p = new Player(name: "aa", 30, 6, 8, 6, 5, 5, 0, 0);

            Battle.WinRate(smile, p, 10000);*/
        }
    }
}
