using System;
using Utils;
using VariousEntity;
using VariousItem;
using Spectre.Console;

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
                TUI.print_stat(c2);

                // 공격
                // 민첩이 높은 크리쳐가 우선 공격
                // 민첩이 같으면 동시 공격
                if (c1.Stat.Agility > c2.Stat.Agility)
                {
                    info = c1.Attack(c2, c1.Abilities[r.Next(c1.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.ColorPrint(255, 255, 0, "크리티컬 히트!!!");
                    }
                    Console.WriteLine($"{c1.Name}(이)가 {c2.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c2.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c2.Name}(은)는 쓰러졌다!");
                        return 1;
                    }
                    info = c2.Attack(c1, c2.Abilities[r.Next(c2.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.ColorPrint(255, 255, 0, "크리티컬 히트!!!");
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
                    info = c2.Attack(c1, c2.Abilities[r.Next(c2.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.ColorPrint(255, 255, 0, "크리티컬 히트!!!");
                    }
                    Console.WriteLine($"{c2.Name}(이)가 {c1.Name}에게 {info.Damage}의 대미지를 주었다!");
                    if (c1.Stat.HP <= 0)
                    {
                        Console.WriteLine($"{c1.Name}(은)는 쓰러졌다!");
                        return 2;
                    }
                    info = c1.Attack(c2, c1.Abilities[r.Next(c1.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.ColorPrint(255, 255, 0, "크리티컬 히트!!!");
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
                    info = c1.Attack(c2, c1.Abilities[r.Next(c1.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.ColorPrint(255, 255, 0, "크리티컬 히트!!!");
                    }
                    Console.WriteLine($"{c1.Name}(이)가 {c2.Name}에게 {info.Damage}의 대미지를 주었다!");
                    info = c2.Attack(c1, c2.Abilities[r.Next(c2.Abilities.Count)]);
                    if (info.IsCritical)
                    {
                        TUI.ColorPrint(255, 255, 0, "크리티컬 히트!!!");
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
        /// 두 엔티티를 count번 만큼 싸우게 하고, 각각의 승리 횟수를 출력함
        /// </summary>
        /// <returns>c1이 이긴 횟수, c2가 이긴 횟수, 무승부 횟수를 튜플 형태로 반환</returns>
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
                        info = fighter1.Attack(fighter2, fighter1.Abilities[r.Next(fighter1.Abilities.Count)]);
                        if (fighter2.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 1;
                            break;
                        }
                        info = fighter2.Attack(fighter1, fighter2.Abilities[r.Next(fighter2.Abilities.Count)]);
                        if (fighter1.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 2;
                            break;
                        }
                    }
                    else if (fighter1.Stat.Agility < fighter2.Stat.Agility)
                    {
                        info = fighter2.Attack(fighter1, fighter2.Abilities[r.Next(fighter2.Abilities.Count)]);
                        if (fighter1.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 2;
                            break;
                        }
                        info = fighter1.Attack(fighter2, fighter1.Abilities[r.Next(fighter1.Abilities.Count)]);
                        if (fighter2.Stat.HP <= 0)
                        {
                            WinnerOfCurrentGame = 1;
                            break;
                        }
                    }
                    else
                    {
                        info = fighter1.Attack(fighter2, fighter1.Abilities[r.Next(fighter1.Abilities.Count)]);
                        info = fighter2.Attack(fighter1, fighter2.Abilities[r.Next(fighter2.Abilities.Count)]);
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
        /*
        public static int PlayerVSEnemy(Player p, Creature c)
        {
            if (p == null) throw new ArgumentNullException("플레이어는 null일 수 없습니다.");
            if (c == null) throw new ArgumentNullException("적은 null일 수 없습니다.");
            
            int turn = 0;
            int result = 0;
            Random r = new Random();
            AttackInfo info;

            while (true)
            {
                turn++;
                // 효과 적용
                p.ApplyEffect();
                c.ApplyEffect();

                // 턴 수 출력
                Console.WriteLine("턴 " + turn);

                // 현재 남은 hp, mp 출력
                TUI.print_stat(c);
                TUI.print_stat(p);
                
                // 플레이어의 행동을 선택
                // Ask for the user's favorite fruit
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[green]{p.Name}[/]은(는) 무엇을 할까?")
                    .PageSize(10)
                    .AddChoices(new[] {
                    "공격", "스킬", "행동",
                    "아이템", "도망가기",
                }));
                if (choice == "공격")
                {
                    // 무기가 없음
                    if (p.EquippedWeapon == null)
                    {
                        var choice2 = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title($"[green]{p.Name}[/]은(는) 어떤 공격을 할까?")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "일반 공격", "강공", "속공"
                        }));
                    }
                    // 무기가 있음
                    else
                    {
                        var choice2 = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title($"[green]{p.Name}[/]은(는) 어떤 공격을 할까?")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "일반 공격", "강공", "속공"
                        }));

                        switch (choice2)
                        {
                            case "일반 공격":
                                info = p.Attack(ref c, new WeaponSkill("일반 공격", p.EquippedWeapon, p.Stat, 1.0));
                                break;
                            case "강공":
                                info = p.Attack(ref c, new WeaponSkill("강공", p.EquippedWeapon, p.Stat, 1.4));
                                break;
                            case "속공":
                                info = p.Attack(ref c, new WeaponSkill("속공", p.EquippedWeapon, p.Stat, 0.7));
                                break;
                        }

                        //AnsiConsole.WriteLine($"{p.Name}(이)가 {c.Name}에게 {info.Damage}의 대미지를 주었다!");
                    }
                }
                else if (choice == "스킬")
                {
                    var choice2 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[green]{p.Name}[/]은(는) 어떤 스킬을 사용할까?")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "일반 공격", "강공", "속공"
                    }));
                }
                else if (choice == "행동")
                {
                    var choice2 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[green]{p.Name}[/]은(는) 어떤 행동을 사용할까?")
                        .PageSize(10)
                        .AddChoices(new[] {
                        "방어 태세"
                    }));
                    if (choice2 == "방어 태세")
                    {

                    }
                }
                else if (choice == "아이템")
                {
                    // 아이템 - 인벤토리 내에 있는 Consumable 목록을 가져옴
                    var choice2 = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[green]{p.Name}[/]은(는) 어떤 아이템을 사용할까?")
                        .PageSize(10)
                        .AddChoices(p.Inventory.Keys 
                                    .Where((item) => item is Consumable) 
                                    .Select((item) => $"{item.Name} x {p.Inventory[]}")
                    );
                }
                else if (choice == "도망가기")
                {
                    // 도망 시도 
                    // 50% * (플레이어 민첩) / (상대 민첩) 확률로 성공
                    if (r.NextDouble() < 0.5 * (double)p.Stat.Agility / (double)c.Stat.Agility)
                    {
                        AnsiConsole.WriteLine($"[green]{p.Name}[/](은)는 무사히 도망쳤다!");
                        break;
                    }
                    // 도망 실패 시 턴을 소비한 것으로 간주함
                }
            }
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker">공격하는 크리쳐</param>
        /// <param name="defender">공격받는 크리쳐</param>
        /// <returns>공격자가 상대를 쓰러뜨리면 1, 그외에는 0을 반환한다.</returns>
        private int TryAttack(ref Creature attacker, ref Creature defender)
        {
            return 0;
        }
        */
    }
}
