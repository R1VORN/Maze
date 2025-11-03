using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.RobotControl
{
    internal class PathFinder
    {
        // Главная функция: ищет минимальный путь через все предметы (перебор всех последовательностей)
        public static List<Position> FindMinimalPath(int[,] maze)
        {
            Position player = FindPlayer(maze);
            List<Position> items = FindAllItems(maze);

            if (player == null || items.Count == 0)
                return null;

            List<Position> bestPath = null;
            int minSteps = int.MaxValue;

            foreach (var perm in GetPermutations(items))
            {
                List<Position> path = new List<Position>();
                Position cur = player;
                bool valid = true;

                foreach (var item in perm)
                {
                    List<Position> segment = BFS(maze, cur, item);
                    if (segment == null)
                    {
                        valid = false; // недостижимый предмет
                        break;
                    }

                    // убираем повторяющуюся стартовую позицию сегмента
                    if (path.Count > 0)
                        segment.RemoveAt(0);

                    path.AddRange(segment);
                    cur = item;
                }

                if (valid)
                {
                    int steps = CountSteps(path);
                    if (steps < minSteps)
                    {
                        minSteps = steps;
                        bestPath = path;
                    }
                }
            }

            return bestPath;
        }

        // Генерация всех перестановок предметов
        private static IEnumerable<List<Position>> GetPermutations(List<Position> items)
        {
            if (items.Count == 1)
            {
                yield return new List<Position> { items[0] };
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var current = items[i];
                    var remaining = items.Where((x, idx) => idx != i).ToList();

                    foreach (var perm in GetPermutations(remaining))
                    {
                        var result = new List<Position> { current };
                        result.AddRange(perm);
                        yield return result;
                    }
                }
            }
        }

        // Подсчёт шагов
        public static int CountSteps(List<Position> path)
        {
            if (path == null || path.Count == 0)
                return 0;
            return path.Count - 1;
        }

        // Проверка, достижимы ли все предметы
        public static int[,] IsMazePassable(int[,] maze)
        {
            Position player = FindPlayer(maze);
            //if (player == null) return false;

            List<Position> items = FindAllItems(maze);
            foreach (var item in items)
            {
                if (!IsItemReachable(maze, player, item))
                {
                    maze[item.X, item.Y] = 0;
                }
                //return false;
            }
            //return true;
            return maze;
        }

        private static bool IsItemReachable(int[,] maze, Position start, Position item)
        {
            return BFS(maze, start, item) != null;
        }

        // BFS — поиск кратчайшего пути между двумя точками
        private static List<Position> BFS(int[,] maze, Position start, Position goal)
        {
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);

            bool[,] visited = new bool[rows, cols];
            Position[,] parent = new Position[rows, cols];
            Queue<Position> q = new Queue<Position>();

            q.Enqueue(start);
            visited[start.X, start.Y] = true;

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (q.Count > 0)
            {
                Position cur = q.Dequeue();

                if (cur.X == goal.X && cur.Y == goal.Y)
                {
                    List<Position> path = new List<Position>();
                    Position p = cur;
                    while (p != null)
                    {
                        path.Add(p);
                        p = parent[p.X, p.Y];
                    }
                    path.Reverse();
                    return path;
                }

                for (int i = 0; i < 4; i++)
                {
                    int nx = cur.X + dx[i];
                    int ny = cur.Y + dy[i];

                    if (nx >= 0 && ny >= 0 && nx < rows && ny < cols)
                    {
                        if (!visited[nx, ny] && maze[nx, ny] != 1)
                        {
                            visited[nx, ny] = true;
                            parent[nx, ny] = cur;
                            q.Enqueue(new Position(nx, ny));
                        }
                    }
                }
            }

            return null;
        }

        public static Position FindPlayer(int[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[i, j] == 3)
                        return new Position(i, j);
            return null;
        }

        public static List<Position> FindAllItems(int[,] maze)
        {
            List<Position> items = new List<Position>();
            for (int i = 0; i < maze.GetLength(0); i++)
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (maze[i, j] == 2)
                        items.Add(new Position(i, j));
            return items;
        }

        public static void PrintFindedPath(List<Position> minimalPath, int steps) 
        {
            int cnt = 0;

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
        }
        public static List<Position> FindFullTraversalPath(int[,] maze)
        {
            // Можно использовать BFS или DFS для обхода всех клеток
            // Простая реализация — пройти все свободные клетки без повторений
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
            bool[,] visited = new bool[rows, cols];
            List<Position> path = new List<Position>();

            Position start = FindPlayer(maze);
            DFSFullTraversal(maze, start, visited, path);
            return path;
        }

        private static void DFSFullTraversal(int[,] maze, Position pos, bool[,] visited, List<Position> path)
        {
            visited[pos.X, pos.Y] = true;
            path.Add(pos);

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = pos.X + dx[i];
                int ny = pos.Y + dy[i];

                if (nx >= 0 && ny >= 0 && nx < maze.GetLength(0) && ny < maze.GetLength(1))
                {
                    if (!visited[nx, ny] && maze[nx, ny] != 1)
                        DFSFullTraversal(maze, new Position(nx, ny), visited, path);
                }
            }
        }

        public static List<Position> FindPath(Position start, Position goal, int[,] maze)
        {
            return BFS(maze, start, goal);
        }
    }
}
