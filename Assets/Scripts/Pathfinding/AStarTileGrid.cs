using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class AStarTileGrid : Grid<AStarTile>
    {
        private readonly TileGrid tiles;

        public AStarTileGrid(int x, int y, ref TileGrid tiles) : base(x, y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    items[i, j] = new AStarTile();
                    items[i, j].location = new Vector2Int(i, j);
                }
            }
            this.tiles = tiles;
        }

        public List<AStarTile> GetAdjacentEmptyTiles(Vector2Int location)
        {
            List<AStarTile> adjacentTiles = GetAdjacentItems(location);
            for (short i = 0; i < adjacentTiles.Count; i++)
            {
                if (tiles[adjacentTiles[i].location].isObstacle)
                {
                    adjacentTiles.RemoveAt(i);
                }
            }

            return adjacentTiles;
        }
    }
}
