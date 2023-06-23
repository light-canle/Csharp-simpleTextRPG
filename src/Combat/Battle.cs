using System;
using System.Security.Cryptography.X509Certificates;
using Utils;
using VariousEntity;
using VariousItem;

namespace Combat
{
    public class Battle
    {
        /// <summary>
        /// 두 크리쳐가 1v1로 싸우는 것을 시뮬레이션 하는 함수
        /// </summary>
        /// <returns>c1이 이기면 1, c2가 이기면 2를 반환한다. 무승부는 0을 반환한다.</returns>
        public static int Battle1v1(Creature c1, Creature c2)
        {
            int turn = 0;
            Random r = new Random();
            AttackInfo info;
            while (true)
            {
                turn++;
                // 효과 적용
                c1.ApplyEffect();
                c2.ApplyEffect();

                // 턴 수 출력
                Console.WriteLine("턴 " + turn);

                // 현재 남은 hp, mp 출력
                TUI.print_stat(c1);
                Console.WriteLine();
                TUI.print_stat(c2);
                Console.WriteLine();

                // 공격
                // 민첩이 높은 크리쳐가 우선 공격
                // 민첩이 같으면 동시 공격
                if (c1.Stat.Agility > c2.Stat.Agility)
                {
                    info = c1.Attack(ref c2, c1.Abilities[r.Next(c1.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.TextColor(255, 255, 0);
                        Console.WriteLine("크리티컬 히트!!!");
                        TUI.TextColor(255, 255, 255);
                    }
                    Console.WriteLine($"{c1.Name}(이)가 {c2.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c2.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c2.Name}(은)는 쓰러졌다!");
                        return 1;
                    }
                    info = c2.Attack(ref c1, c2.Abilities[r.Next(c2.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.TextColor(255, 255, 0);
                        Console.WriteLine("크리티컬 히트!!!");
                        TUI.TextColor(255, 255, 255);
                    }
                    Console.WriteLine($"{c2.Name}(이)가 {c1.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c1.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c1.Name}(은)는 쓰러졌다!");
                        return 2;
                    }
                }
                else if (c1.Stat.Agility < c2.Stat.Agility)
                {
                    info = c2.Attack(ref c1, c2.Abilities[r.Next(c2.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.TextColor(255, 255, 0);
                        Console.WriteLine("크리티컬 히트!!!");
                        TUI.TextColor(255, 255, 255);
                    }
                    Console.WriteLine($"{c2.Name}(이)가 {c1.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c1.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c1.Name}(은)는 쓰러졌다!");
                        return 2;
                    }
                    info = c1.Attack(ref c2, c1.Abilities[r.Next(c1.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.TextColor(255, 255, 0);
                        Console.WriteLine("크리티컬 히트!!!");
                        TUI.TextColor(255, 255, 255);
                    }
                    Console.WriteLine($"{c1.Name}(이)가 {c2.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c2.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c2.Name}(은)는 쓰러졌다!");
                        return 1;
                    }
                }
                else
                {
                    info = c1.Attack(ref c2, c1.Abilities[r.Next(c1.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.TextColor(255, 255, 0);
                        Console.WriteLine("크리티컬 히트!!!");
                        TUI.TextColor(255, 255, 255);
                    }
                    Console.WriteLine($"{c1.Name}(이)가 {c2.Name}에게 {info.Damage}의 대미지를 주었다!");
                    info = c2.Attack(ref c1, c2.Abilities[r.Next(c2.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.TextColor(255, 255, 0);
                        Console.WriteLine("크리티컬 히트!!!");
                        TUI.TextColor(255, 255, 255);
                    }
                    Console.WriteLine($"{c2.Name}(이)가 {c1.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c1.Stat.HP <= 0 && c2.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c1.Name}(과)와 {c2.Name}(은)는 동시에 쓰러졌다!");
                        return 0;
                    }
                    if (c1.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c1.Name}(은)는 쓰러졌다!");
                        return 2;
                    }
                    if (c2.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c2.Name}(은)는 쓰러졌다!");
                        return 1;
                    }
                }

                Console.WriteLine("=================================");
            }
        }
    }
}
