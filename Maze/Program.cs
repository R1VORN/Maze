using Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            Console.WriteLine();
            Console.WriteLine();

            int[,] maze1 = GenerateMazes.GenerateMaze(density);
            GenerateMazes.PlaceItemsAndPlayer(maze1);
            DrawMazes.DrawMaze(maze1);
            Console.WriteLine();

            int[,] verifyMaze = PathFinder.IsMazePassable(maze1);
            /*DrawMaze(verifyMaze);
            Console.WriteLine();*/



            int itemCount = (PathFinder.FindAllItems(verifyMaze)).Count;

            if (itemCount == 0)
            {
                Console.WriteLine("Нет достижимых предметов.");
                return;
            }

            // Ищем минимальный путь
            List<Position> minimalPath = PathFinder.FindMinimalPath(maze1);
            int steps = PathFinder.CountSteps(minimalPath);

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
                Console.Write($"({minimalPath[i].Y},{minimalPath[i].X})");
                if (i != (steps - 1))
                {
                    Console.Write(" —> ");
                }
            }
            Console.WriteLine();
        }
    }
}