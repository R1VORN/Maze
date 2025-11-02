using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    internal static class GenerateMazes
    {
        public static Random random = new Random();
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

        public static void CreatePath(int[,] maze, int startX, int startY, int endX, int endY)
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
