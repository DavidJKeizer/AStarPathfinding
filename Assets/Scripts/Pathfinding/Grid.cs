using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Grid<T>
    {
        protected T[,] items;

        public T this[int i, int j]
        {
            get
            {
                return items[i, j];
            }
            set
            {
                items[i, j] = value;
            }
        }

        public T this[Vector2Int location]
        {
            get
            {
                return items[location.x, location.y];
            }
            set
            {
                items[location.x, location.y] = value;
            }
        }

        public Grid(int x, int y)
        {
            items = new T[x, y];
        }

        public List<T> GetAdjacentItems(Vector2Int location, bool allowDiagonals = true)
        {
            List<T> adjacentItems = new List<T>();
            int xLoc, yLoc;

            for (short dx = -1; dx <= 1; ++dx)
            {
                for (short dy = -1; dy <= 1; ++dy)
                {
                    xLoc = location.x + dx;
                    yLoc = location.y + dy;


                    if (xLoc < 0 || xLoc >= items.GetLength(0))
                    {
                        continue; //Item is out of range
                    }

                    if (yLoc < 0 || yLoc >= items.GetLength(1))
                    {
                        continue; //Item is out of range
                    }

                     if (dx != 0 || dy != 0)
                    {
                        adjacentItems.Add(items[xLoc, yLoc]);
                    }


                }
            }

            return adjacentItems;
        }

        /// <summary>
        /// Calls f foreach item, and replaces the item with the result.
        /// </summary>
        /// <param name="f"></param>
        public void EditForEeachItem(Func<T, T> f)
        {
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    items[i, j] = f(items[i, j]);
                }
            }
        }

        /// <summary>
        /// Calls action for each item. Does not affect tiles[i, j].
        /// </summary>
        /// <param name="action"></param>
        public void ReadForEachItem(Action<T> action)
        {
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    action(items[i, j]);
                }
            }
        }


    }
}

