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
        public static Random random = new Random();

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
                        case 0: // Новая игра
                            RunBeforeGameMenu();
                            break;

                        case 1: // Прохождение роботом (отдельный пункт)
                            Console.Clear();
                            Console.WriteLine("Функция 'Прохождение роботом' запустится после игры.");
                            Console.ReadKey(true);
                            break;

                        case 2: // Выход
                            programRunning = false;
                            break;
                    }
                }
            }

            Console.Clear();
            Console.WriteLine("Выход из программы...");
            Thread.Sleep(500);
        }

        static void RunBeforeGameMenu()
        {
            bool inBeforeMenu = true;
            int densityChoice = 0;
            int density = 10;

            while (inBeforeMenu)
            {
                Console.Clear();
                BeforeGameMenu.PrintOption(densityChoice);
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    inBeforeMenu = false; // возврат в главное меню
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    // Определяем плотность по выбору
                    switch (densityChoice)
                    {
                        case 0:
                            density = random.Next(10, 17);
                            break;
                        case 1:
                            density = random.Next(17, 25);
                            break;
                        case 2:
                            density = random.Next(25, 31);
                            break;
                    }

                    Console.Clear();
                    Console.WriteLine($"Выбрана плотность: {BeforeGameMenu.GetDensityName(densityChoice)}");
                    Thread.Sleep(1000);

                    RunGame(density, BeforeGameMenu.GetDensityName(densityChoice));
                    inBeforeMenu = false;
                }
                else
                {
                    BeforeGameMenu.ChoiceDe(key, ref densityChoice);
                }
            }
        }

        static void RunGame(int density, string densityName)
        {
            Console.Clear();
            Console.WriteLine($"Плотность застройки: {densityName}");
            Thread.Sleep(500);

            int[,] maze = GenerateMazes.GenerateMaze(density);
            Console.WriteLine();
            GenerateMazes.PlaceItemsAndPlayer(maze);
            Console.SetCursorPosition(0, 4);
            DrawMazes.DrawMaze(maze);
            int[,] verifyMaze = PathFinder.IsMazePassable(maze);

            bool isRunning = true;
            Console.CursorVisible = false;
            ConsoleKeyInfo pressedKey;

            Position playerPosition = PathFinder.FindPlayer(maze);

            List<Position> minimalPath = PathFinder.FindMinimalPath(verifyMaze);
            int steps = PathFinder.CountSteps(minimalPath);
            List<Position> itemPlaces = PathFinder.FindAllItems(verifyMaze);
            List<Position> itemPlacesLater = new List<Position>();
            int itemCount = itemPlaces.Count;
            for (int i = 0; i <= itemCount - 1; i++)
            {
                itemPlacesLater.Add(itemPlaces[i]);
            }
            int itemGeted = 0;
            int playerSteps = 0;

            GameInterface.PrintGame(maze, itemGeted, itemCount, playerSteps);

            while (isRunning)
            {
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true);
                    PlayerMovements.PlayerMovement(pressedKey, ref playerPosition, ref maze,
                        ref isRunning, ref itemPlaces, ref itemGeted, ref playerSteps);
                }

                GameInterface.PrintGame(maze, itemGeted, itemCount, playerSteps);
                Thread.Sleep(50);

                if (itemCount == itemGeted)
                {
                    isRunning = false;
                }
            }

            GameInterface.PrintGame(maze, itemGeted, itemCount, playerSteps);

            if (itemCount == 0)
            {
                Console.WriteLine("Нет достижимых предметов.");
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine($"\nЛабиринт пройден!");
            Console.WriteLine($"Предметов собрано: {itemGeted}/{itemCount}");
            Console.WriteLine($"Оптимальное количество шагов для сбора всех предметов: {steps}");
            Console.WriteLine($"Ваши шаги: {playerSteps}");
            Console.WriteLine("\nПуть игрока (X, Y):");
            PathFinder.PrintFindedPath(minimalPath, steps);

            Console.WriteLine("\nХотите, чтобы робот прошёл этот уровень оптимальным маршрутом? (Y/N)");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
            {
                RunRobotPath(verifyMaze, minimalPath, itemPlacesLater);
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
            Console.ReadKey(true);
        }

        static void RunRobotPath(int[,] maze, List<Position> path, List<Position> items)
        {
            Console.Clear();
            Console.WriteLine("Робот начал прохождение...");
            Position playerPos = PathFinder.FindPlayer(maze);
            maze[playerPos.X, playerPos.Y] = 0;
            maze[path[0].X, path[0].Y] = 0;
            for (int i = 0; i <= items.Count - 1; i++)
            {
                maze[items[i].X, items[i].Y] = 2;
            }
            RobotMethods.AnimationPathMaze(maze, path);

            Console.WriteLine("\nРобот прошёл не коротким, а оптимальным маршрутом!");
            Console.WriteLine($"Шаги робота: {path.Count}");
            Console.WriteLine("\nПуть путь робота (X, Y):");
            PathFinder.PrintFindedPath(path, path.Count);
            Console.ReadKey(true);
        }
    }
}
