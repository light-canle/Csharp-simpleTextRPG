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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static (int, int, int) WinRate(Creature c1, Creature c2, int count)
        {
            if (count <= 0) throw new ArgumentException("count는 양수여야만 합니다.");
            Creature fighter1 = c1.Clone();
            Creature fighter2 = c2.Clone();
            int f1Win = 0;
            int f2Win = 0;
            int draw = 0;
            int WinnerOfCurrentGame = -1;
            Random r = new Random();
            AttackInfo info;
            for (int i = 0; i < count; i++)
            {
                fighter1.Stat.HP = fighter1.Stat.MaxHP;
                fighter1.Stat.MP = fighter1.Stat.MaxMP;
                fighter1.Effects.Clear();

                fighter2.Stat.HP = fighter2.Stat.MaxHP;
                fighter2.Stat.MP = fighter2.Stat.MaxMP;
                fighter2.Effects.Clear();
                while (true)
                {
                    // 효과 적용
                    fighter1.ApplyEffect();
                    fighter2.ApplyEffect();

                    // 공격
                    // 민첩이 높은 크리쳐가 우선 공격
                    // 민첩이 같으면 동시 공격
                    if (fighter1.Stat.Agility > fighter2.Stat.Agility)
                    {
                        info = fighter1.Attack(ref fighter2, fighter1.Abilities[r.Next(fighter1.Abilities.Count)]);
                        if (fighter2.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 1;
                            break;
                        }
                        info = fighter2.Attack(ref fighter1, fighter2.Abilities[r.Next(fighter2.Abilities.Count)]);
                        if (fighter1.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 2;
                            break;
                        }
                    }
                    else if (fighter1.Stat.Agility < fighter2.Stat.Agility)
                    {
                        info = fighter2.Attack(ref fighter1, fighter2.Abilities[r.Next(fighter2.Abilities.Count)]);
                        if (fighter1.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 2;
                            break;
                        }
                        info = fighter1.Attack(ref fighter2, fighter1.Abilities[r.Next(fighter1.Abilities.Count)]);
                        if (fighter2.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 1;
                            break;
                        }
                    }
                    else
                    {
                        info = fighter1.Attack(ref fighter2, fighter1.Abilities[r.Next(fighter1.Abilities.Count)]);
                        info = fighter2.Attack(ref fighter1, fighter2.Abilities[r.Next(fighter2.Abilities.Count)]);
                        if (fighter1.Stat.HP <= 0 && fighter2.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 0;
                            break;
                        }
                        if (fighter1.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 2;
                            break;
                        }
                        if (fighter2.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 1;
                            break;
                        }
                    }
                }
                switch(WinnerOfCurrentGame)
                {
                    case 0: draw++; break;
                    case 1: f1Win++; break;
                    case 2: f2Win++; break;
                }
            }
                
            return (f1Win, f2Win, draw);
        } 
    }
}
