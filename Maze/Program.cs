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

        // дефолтный цвет символа в консоли
        public static ConsoleColor defaultColor = Console.ForegroundColor;

        // символы для типов ячеек
        public static class MazeSymbols
        {
            public const char Wall = '█';      // U+2588
            public const char Empty = '.';     // U+0020
            public const char Item = '$';
            public const char Player = '@';
        }
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

            int[,] maze1 = GenerateMaze(density);
            PlaceItemsAndPlayer(maze1);
            DrawMaze(maze1);
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




        public static int[,] GenerateMaze(int density)
        {
            if (density < 0 || density > 30)
            {
                Console.WriteLine("Введена некорректная плотность застройки (она должна быть от 0 до 30).");
                return null;
            }
            else
            {
                int[,] maze = new int[13, 13];
                int height = maze.GetLength(0);
                int width = maze.GetLength(1);

                // 1. Сначала делаем все клетки проходами
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        maze[y, x] = 0;

                // 2. Границы - стены
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            maze[y, x] = 1;
                        }
                    }
                }

                // 3. Гарантированный путь от старта (1,1) до всех углов
                CreatePath(maze, 1, 1, width - 2, 1);      // Путь вправо
                CreatePath(maze, 1, 1, 1, height - 2);     // Путь вниз
                CreatePath(maze, 1, 1, width - 2, height - 2); // Путь по диагонали

                // 4. Добавляем случайные стены согласно плотности
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        if (maze[y, x] == 0 && random.Next(100) < density)
                        {
                            maze[y, x] = 1;
                        }
                    }
                }

                return maze;
            }
        }

        // Простой метод создания пути между двумя точками
        private static void CreatePath(int[,] maze, int startX, int startY, int endX, int endY)
        {
            int x = startX, y = startY;

            while (x != endX || y != endY)
            {
                maze[y, x] = 0; // Делаем клетку проходом

                if (x < endX) x++;
                else if (x > endX) x--;
                else if (y < endY) y++;
                else if (y > endY) y--;
            }
            maze[endY, endX] = 0;
        }

        /// <summary>
        /// Отрисовка лабиринта в консоли
        /// </summary>
        /// <param name="maze"></param>
        public static void DrawMaze(int[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Console.Write(CellTypeConverter(maze[i, j]));
                    Console.ForegroundColor = defaultColor;
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Конвертер для отрисовки типов ячеек
        /// </summary>
        /// <param name="cellType"></param>
        /// <returns></returns>
        public static char CellTypeConverter(int cellType)
        {
            switch (cellType)
            {
                case 0:
                    return MazeSymbols.Empty;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    return MazeSymbols.Wall;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return MazeSymbols.Item;
                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return MazeSymbols.Player;
                default:
                    return MazeSymbols.Empty;
            }
        }


        public static void PlaceItemsAndPlayer(int[,] maze)
        {
            Random rand = new Random();

            int playerX;
            int playerY;

            while (true)
            {
                playerX = rand.Next(1, 11);
                playerY = rand.Next(1, 11);

                if (maze[playerY, playerX] != 1)
                {
                    maze[playerY, playerX] = 3;
                    break;
                }
            }

            // Генерация случайного количества предметов (от 3 до 7)
            int itemCount = rand.Next(3, 8);
            int placedItems = 0; // Счётчик размещённых предметов

            while (placedItems < itemCount)
            {
                int x = rand.Next(1, maze.GetLength(0) - 1);
                int y = rand.Next(1, maze.GetLength(1) - 1);

                // Проверяем, пустая ли клетка
                if (maze[x, y] == 0 & !(x == playerX & y == playerY))
                {
                    // Ставим случайный предмет
                    //maze[x, y] = items[rand.Next(items.Length)][0];

                    maze[x, y] = 2;

                    placedItems++; // Увеличиваем счётчик размещённых предметов
                }
            }
        }
    }
}