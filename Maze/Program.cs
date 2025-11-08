using Maze.Control;
using Maze.MazeFunctions;
using Maze.Menu;
using Maze.RobotControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Maze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bool programRunning = true;
            int mainChoice = 0;

            while (programRunning)
            {
                Console.Clear();
                MainMenu.PrintMenu(mainChoice);
                var key = Console.ReadKey(true);
                MainMenu.ChoiceAction(key, ref mainChoice);

                if (key.Key == ConsoleKey.Enter)
                {
                    switch (mainChoice)
                    {
                        case 0:
                            MenuModes.RunBeforeGameMenu();
                            break;

                        case 1:
                            MenuModes.RunRobotFullTraversal();
                            break;

                        case 2:
                            programRunning = false;
                            break;
                    }
                }
            }

            Console.Clear();
            Console.WriteLine("Выход из программы...");
            Thread.Sleep(500);
        }
    }
}