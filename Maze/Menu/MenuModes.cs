using Maze.Control;
using Maze.MazeFunctions;
using Maze.RobotControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maze.Menu
{
    static class MenuModes
    {
        public static Random random = new Random();
        public static void RunBeforeGameMenu()
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
                    inBeforeMenu = false;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
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

        public static void RunGame(int density, string densityName)
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

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
            {
                RunRobotPath(verifyMaze, minimalPath, itemPlacesLater);
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
            Console.ReadKey(true);
        }

        public static void RunRobotPath(int[,] maze, List<Position> path, List<Position> items)
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

        public static void RunRobotFullTraversal()
        {
            Console.Clear();
            Console.WriteLine("Робот начинает обход лабиринта...");
            Thread.Sleep(500);

            int density = random.Next(10, 25);
            int[,] maze = GenerateMazes.GenerateMaze(density);
            GenerateMazes.PlaceItemsAndPlayer(maze);

            Position startPosition = PathFinder.FindPlayer(maze);

            int[,] verifyMaze = PathFinder.IsMazePassable(maze);
            List<Position> items = PathFinder.FindAllItems(verifyMaze);

            List<Position> fullTraversalPath = PathFinder.FindFullTraversalPath(verifyMaze);
            RobotMethods.AnimationPathMazeWithoutCollectingItems(maze, fullTraversalPath, items);

            Console.WriteLine("\nРобот завершил обход всего лабиринта.");
            Thread.Sleep(1000);

            RobotMethods.AnimationReturnToStart(maze, PathFinder.FindPlayer(maze), startPosition, items);

            List<Position> minimalPath = PathFinder.FindMinimalPath(verifyMaze);
            RobotMethods.AnimationPathMaze(maze, minimalPath);

            Console.WriteLine("\nРобот завершил оптимальный маршрут.");
            Console.WriteLine($"Шаги робота: {minimalPath.Count}");
            Console.WriteLine("\nПуть робота (X, Y):");
            PathFinder.PrintFindedPath(minimalPath, minimalPath.Count);

            Console.WriteLine("\nНажмите любую клавишу для возврата в главное меню...");
            Console.ReadKey(true);
        }
    }
}
