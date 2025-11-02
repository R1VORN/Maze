using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Menu
{
    internal static class MainMenu
    {
        static string[] nameLines = { "Новая игра", "Прохождение роботом", "Настройки", "Выход" };
        public static void PrintMenu(int choice)
        {
            for (int i = 0; i < nameLines.Length - 1; i++)
            {
                Console.WriteLine(nameLines[i]);
            }

            Console.SetCursorPosition(nameLines[choice].Length + 2, choice);
            Console.Write("⯇");
        }

        public static void ChoiceAction(ConsoleKeyInfo keyPressed, int choice)
        {
            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    if (choice < nameLines.Length - 1)
                    {
                        choice++;
                    }
                    else
                    {
                        choice = 0;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (choice > 0)
                    {
                        choice--;
                    }
                    else
                    {
                        choice = nameLines.Length - 1;
                    }
                    break;
            }
        }
    }
}
