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

            Console.SetCursorPosition(0, 4);
            DrawMazes.DrawMaze(maze);
            Thread.Sleep(200);

            for (int i = 1; i < path.Count; i++)
            {
                maze[path[i - 1].X, path[i - 1].Y] = 0;

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

        public static void AnimationPathMazeWithoutCollectingItems(int[,] maze, List<Position> path, List<Position> items)
        {
            if (path == null || path.Count == 0) return;

            foreach (var item in items)
                maze[item.X, item.Y] = 2;

            Console.SetCursorPosition(0, 4);
            DrawMazes.DrawMaze(maze);
            Thread.Sleep(200);

            for (int i = 1; i < path.Count; i++)
            {
                maze[path[i - 1].X, path[i - 1].Y] = 0;
                maze[path[i].X, path[i].Y] = 3;

                foreach (Position item in items)
                {
                    if (maze[item.X, item.Y] != 3)
                        maze[item.X, item.Y] = 2;
                }

                Console.SetCursorPosition(0, 4);
                DrawMazes.DrawMaze(maze);
                Thread.Sleep(100);
            }

            MazeAfterAnimationPathMaze(maze);
        }

        public static void AnimationReturnToStart(int[,] maze, Position currentPos, Position startPos, List<Position> items)
        {
            List<Position> returnPath = PathFinder.FindPath(currentPos, startPos, maze);
            foreach (var pos in returnPath)
            {
                maze[currentPos.X, currentPos.Y] = 0;
                maze[pos.X, pos.Y] = 3;

                foreach (var item in items)
                {
                    if (maze[item.X, item.Y] != 3)
                        maze[item.X, item.Y] = 2;
                }

                Console.SetCursorPosition(0, 4);
                DrawMazes.DrawMaze(maze);
                Thread.Sleep(100);
                currentPos = pos;
            }
        }

    }
}