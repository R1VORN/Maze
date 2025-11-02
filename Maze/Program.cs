using Maze;
using Maze.Control;
using Maze.MazeFunctions;
using Maze.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Maze
{
    internal class Program
    {
        public static Random random = new Random();
        static void Main(string[] args)
        {

            int density = 10;
            bool flagMenu = true;
            //bool flagQuit = false;

            string choice = "";

            while (flagMenu)
            {
                Console.WriteLine("Выберите плотность застройки:\n" +
                    "1 — низкая\n" +
                    "2 — средняя\n" +
                    "3 — высокая\n" +
                    "q — выйти из программы\n");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        density = random.Next(10, 17);
                        flagMenu = false;
                        break;
                    case "2":
                        density = random.Next(17, 25);
                        flagMenu = false;
                        break;
                    case "3":
                        density = random.Next(25, 31);
                        flagMenu = false;
                        break;
                }

                if (flagMenu)
                {
                    if (choice.ToLower() == "q")
                    {
                        //flagQuit = true;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Введите корректные данные.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    break;
                }
            }

            Console.Clear();
            Console.Write("Плотность застройки: ");

            switch (choice)
            {
                case "1":
                    Console.Write("низкая");
                    break;
                case "2":
                    Console.Write("средняя");
                    break;
                case "3":
                    Console.Write("высокая");
                    break;
            }


            int[,] maze = GenerateMazes.GenerateMaze(density);
            GenerateMazes.PlaceItemsAndPlayer(maze);
            int[,] verifyMaze = PathFinder.IsMazePassable(maze);

            bool isRunning = true;
            ConsoleKeyInfo pressedKey = Console.ReadKey();
            Position playerPosition = PathFinder.FindPlayer(maze);


            Console.CursorVisible = false;
            List<Position> minimalPath = PathFinder.FindMinimalPath(verifyMaze);
            int steps = PathFinder.CountSteps(minimalPath);
            List<Position> itemPlaces = PathFinder.FindAllItems(verifyMaze);
            int itemCount = itemPlaces.Count;
            int itemGeted = 0;
            int playerSteps = 0;

            GameInterface.PrintGame(maze, itemGeted, itemCount, playerSteps);

            while (isRunning)
            {
                GameInterface.PrintGame(maze, itemGeted, itemCount, playerSteps);

                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true);

                    PlayerMovements.PlayerMovement(pressedKey, ref playerPosition, ref maze,
                        ref isRunning, ref itemPlaces, ref itemGeted, ref playerSteps);
                }

                Thread.Sleep(50);
                
                if (itemCount == itemGeted)
                {
                    isRunning = false;
                }

            }

            GameInterface.PrintGame(maze, itemGeted, itemCount, playerSteps);

            /*DrawMaze(verifyMaze);
            Console.WriteLine();*/

            if (itemCount == 0)
            {
                Console.WriteLine("Нет достижимых предметов.");
                return;
            }


            Console.WriteLine($"Лабиринт проходим. Есть достижимые предметы ({itemCount})!");
            Console.WriteLine($"Минимальное количество шагов для сбора всех предметов: {steps}\n");

            Console.WriteLine("Путь игрока (X, Y):\n");

            byte cnt = 0;

            for (int i = 0; i < steps; i++)
            {
                cnt++;

                if (cnt == 10)
                {
                    Console.WriteLine();
                    cnt = 0;
                }
                Console.Write($"({minimalPath[i].X},{minimalPath[i].Y})");
                if (i != (steps - 1))
                {
                    Console.Write(" —> ");
                }
            }
            Console.WriteLine();
        }
    }
}