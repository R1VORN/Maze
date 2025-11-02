using Maze.MazeFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maze.RobotControl
{
    static class RobotMethods
    {
        public static void AnimationPathMaze(int[,] maze, List<Position> path)
        {
            if (path == null || path.Count == 0)
                return;

            // Отображаем стартовое состояние
            Console.SetCursorPosition(0, 4);
            DrawMazes.DrawMaze(maze);
            Thread.Sleep(200);

            for (int i = 1; i < path.Count; i++) // <-- строго "<", не "<="
            {
                // Очищаем предыдущую позицию
                maze[path[i - 1].X, path[i - 1].Y] = 0;

                // Ставим робота в текущую позицию
                maze[path[i].X, path[i].Y] = 3;

                Console.SetCursorPosition(0, 4);
                DrawMazes.DrawMaze(maze);
                Thread.Sleep(100);
            }

            MazeAfterAnimationPathMaze(maze);
        }
        public static void MazeAfterAnimationPathMaze(int[,] maze)
        {
            Console.SetCursorPosition(0, 4);
            DrawMazes.DrawMaze(maze);
        }
    }
}
