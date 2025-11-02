using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Menu
{
    static class MainMenu
    {
        static string[] nameLines = { "Новая игра", "Прохождение роботом", "Выход" };
        public static void PrintMenu(int choice)
        {
            for (int i = 0; i <= nameLines.Length - 1; i++)
            {
                Console.WriteLine(nameLines[i]);
            }

            Console.SetCursorPosition(nameLines[choice].Length + 2, choice);
            Console.Write("<|");
            Console.SetCursorPosition(0, nameLines.Length + 2);
            Console.Write("Для выбора нажмите Enter");
        }

        public static void ChoiceAction(ConsoleKeyInfo keyPressed,ref int choice)
        {
            switch (keyPressed.Key)
            {
                case ConsoleKey.DownArrow:
                    if (choice < nameLines.Length - 1)
                    {
                        choice++;
                    }
                    else
                    {
                        choice = 0;
                    }
                    break;

                case ConsoleKey.UpArrow:
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
