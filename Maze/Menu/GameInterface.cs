using Maze.MazeFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Menu
{
    static class GameInterface
    {
        public static void PrintGame(int[,] maze, int itemGeted, int itemCount, int playerSteps)
        {
            Console.SetCursorPosition(0, 2);
            Console.Write($"Собрано предметов из доступных: {itemGeted}/{itemCount}");
            Console.SetCursorPosition(0, 3);
            Console.Write($"Сделано шагов: {playerSteps}");
            Console.SetCursorPosition(0, 4);
            DrawMazes.DrawMaze(maze);
            Console.WriteLine();
        }
    }
}
