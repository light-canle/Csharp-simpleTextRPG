using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;
using VariousItem;

namespace Utils
{
    public class UI
    {
        /// <summary>
        /// 콘솔 창의 글자 색을 바꾼다.
        /// </summary>
        /// https://stackoverflow.com/questions/7937256/custom-text-color-in-c-sharp-console-application
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public static void TextColor(int r, int g, int b)
        {
            Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m");
        }

        /// <summary>
        /// 콘솔 창의 배경 색을 바꾼다.
        /// </summary>
        /// https://stackoverflow.com/questions/7937256/custom-text-color-in-c-sharp-console-application
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public static void BackgroundColor(int r, int g, int b)
        {
            Console.Write("\x1b[48;2;" + r + ";" + g + ";" + b + "m");
        }

        /// <summary>
        /// 해당 크리쳐의 hp와 mp를 출력한다.
        /// </summary>
        /// <param name="c">크리쳐</param>
        public static void print_stat(Creature c)
        {
            // HP에 따른 하트 수 계산
            double hp_percent = c.Stat.HP / c.Stat.MaxHP;
            int heart_count = (c.Stat.HP == c.Stat.MaxHP) ? 10 : (int)Math.Ceiling(hp_percent / (1.0 / 9.0));
            string s = "";
            for (int i = 0; i < heart_count; i++) s += "♥";
            for (int i = 0; i < 10-heart_count; i++) s += "♡";
            // 정확한 수치 표시
            s += " " + c.Stat.HP.ToString() + "/" + c.Stat.MaxHP.ToString();
            // 남은 hp 양에 따라 색을 달리함
            if (heart_count > 5) UI.TextColor(0, 255, 0);
            else if (heart_count > 2) UI.TextColor(255, 255, 0);
            else UI.TextColor(255, 0, 0);

            Console.WriteLine(s);

            // MP에 따른 별 개수 계산
            double mp_percent = c.Stat.MP / c.Stat.MaxMP;
            int mp_count = (c.Stat.MP == c.Stat.MaxMP) ? 10 : (int)Math.Ceiling(mp_percent / (1.0 / 9.0));
            s = "";
            for (int i = 0; i < mp_count; i++) s += "★";
            for (int i = 0; i < 10 - mp_count; i++) s += "☆";
            // 수치 표시
            s += " " + c.Stat.MP.ToString() + "/" + c.Stat.MaxMP.ToString();

            UI.TextColor(0, 255, 255);
            Console.WriteLine(s);
        }
    }
}
