using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class TileGrid : Grid<Tile>
    {
       
        public TileGrid(int x, int y) : base(x, y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    items[i, j].Location = new Vector2Int(i, j);
                }
            }
        }

        private Vector2Int GetRandomLocation()
        {
            return new Vector2Int(UnityEngine.Random.Range(0, items.GetLength(0)), UnityEngine.Random.Range(0, items.GetLength(1)));
        }

        public void ClearObstacles()
        {
            for(int i = 0; i < items.GetLength(0); i++)
            {
                for(int j = 0; j < items.GetLength(1); j++)
                {
                    items[i, j].isObstacle = false;
                }
            }
        }

        public void GenerateObstacles(int numberOfObstacles)
        {
            numberOfObstacles = Mathf.Clamp(numberOfObstacles, 0, items.Length);
            Vector2Int randomLocation;
            for (int i = 0; i < numberOfObstacles; i++)
            {
                randomLocation = GetRandomLocation();
                items[randomLocation.x, randomLocation.y].isObstacle = true;
            }
        }


    }
}


