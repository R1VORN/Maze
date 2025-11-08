using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Maze.Control
{
    static class PlayerMovements
    {
        public static void PlayerMovement(ConsoleKeyInfo keyPressed, ref Position player,
            ref int[,] maze, ref bool isRunning, ref List<Position> itemPlaces, ref int itemGeted, ref int playerSteps)
        {
            int x = player.X;
            int y = player.Y;
            Position itemToRemove = null;

            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    if (y > 0 && maze[x, y - 1] != 1)
                    {
                        maze[x, y] = 0;
                        y--;
                        maze[x, y] = 3;
                        playerSteps++;
                    }

                    foreach (Position item in itemPlaces)
                    {
                        if (x == item.X && y == item.Y)
                        {
                            itemGeted += 1;
                            itemToRemove = item;
                            break;
                        }
                    }

                    if (itemToRemove != null)
                    {
                        itemPlaces.Remove(itemToRemove);
                    }

                    break;

                case ConsoleKey.DownArrow:
                    if (y < maze.GetLength(1) - 1 && maze[x, y + 1] != 1)
                    {
                        maze[x, y] = 0;
                        y++;
                        maze[x, y] = 3;
                        playerSteps++;
                    }

                    foreach (Position item in itemPlaces)
                    {
                        if (x == item.X && y == item.Y)
                        {
                            itemGeted += 1;
                            itemToRemove = item;
                            break;
                        }
                    }

                    if (itemToRemove != null)
                    {
                        itemPlaces.Remove(itemToRemove);
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (x > 0 && maze[x - 1, y] != 1)
                    {
                        maze[x, y] = 0;
                        x--;
                        maze[x, y] = 3;
                        playerSteps++;
                    }

                    foreach (Position item in itemPlaces)
                    {
                        if (x == item.X && y == item.Y)
                        {
                            itemGeted += 1;
                            itemToRemove = item;
                            break;
                        }
                    }

                    if (itemToRemove != null)
                    {
                        itemPlaces.Remove(itemToRemove);
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (x < maze.GetLength(0) - 1 && maze[x + 1, y] != 1)
                    {
                        maze[x, y] = 0;
                        x++;
                        maze[x, y] = 3;
                        playerSteps++;
                    }

                    foreach (Position item in itemPlaces)
                    {
                        if (x == item.X && y == item.Y)
                        {
                            itemGeted += 1;
                            itemToRemove = item;
                            break;
                        }
                    }

                    if (itemToRemove != null)
                    {
                        itemPlaces.Remove(itemToRemove);
                    }
                    break;

                case ConsoleKey.Escape:
                    isRunning = false;
                    return;
            }

            player.X = x;
            player.Y = y;
        }
    }
}