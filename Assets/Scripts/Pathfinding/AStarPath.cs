using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class AStarPath
    {
        private readonly List<Tile> path = new List<Tile>();

        private int currentTileIndex = 0;

        public bool GetNextTile(out Tile nextTile)
        {
            if (currentTileIndex + 1 < path.Count)
            {
                nextTile = path[currentTileIndex + 1];
                currentTileIndex++;
                return true;
            }
            nextTile = new Tile();
            return false;

        }

        public void AddTile(Tile tile)
        {
            path.Add(tile);
        }
    }
}
