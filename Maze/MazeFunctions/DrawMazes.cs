using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.MazeFunctions
{
    static class DrawMazes
    {
        public static ConsoleColor defaultColor = Console.ForegroundColor;

        public class MazeSymbols
        {
            public const char Wall = '█';      // U+2588
            public const char Empty = '.';     // U+0020
            public const char Item = '$';
            public const char Player = '@';
        }

        public enum CellType
        {
            Empty = 0,
            Wall = 1,
            Item = 2,
            Player = 3
        }

        public static void DrawMaze(int[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Console.Write(CellTypeConverter(maze[j, i]));
                    Console.ForegroundColor = defaultColor;
                }
                Console.WriteLine();
            }
        }
        public static char CellTypeConverter(int cellType)
        {
            switch (cellType)
            {
                case 0:
                    return DrawMazes.MazeSymbols.Empty;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    return DrawMazes.MazeSymbols.Wall;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return DrawMazes.MazeSymbols.Item;
                case 3:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return DrawMazes.MazeSymbols.Player;
                default:
                    return DrawMazes.MazeSymbols.Empty;
            }
        }
    }
}
