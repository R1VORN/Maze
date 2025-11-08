using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Menu
{
    static class BeforeGameMenu
    {
        static string[] densitys = { "низкая", "средняя", "высокая" };
        public static void PrintOption(int choice)
        {
            Console.WriteLine($"Выберите плотность застройки: {densitys[choice]}");
            Console.WriteLine($"<- -> чтобы изменить, Enter — подтвердить, Escape — назад");
        }
        public static void ChoiceDe(ConsoleKeyInfo keyPressed, ref int choice)
        {
            switch (keyPressed.Key)
            {
                case ConsoleKey.RightArrow:
                    if (choice < densitys.Length - 1)
                    {
                        choice++;
                    }
                    else
                    {
                        choice = 0;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (choice > 0)
                    {
                        choice--;
                    }
                    else
                    {
                        choice = densitys.Length - 1;
                    }
                    break;
            }
        }
        public static string GetDensityName(int choice)
        {
            return densitys[choice];
        }
    }
}