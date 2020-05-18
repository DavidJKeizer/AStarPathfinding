using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting.Messaging;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pathfinding
{
    public class AStar 
    {
        /// <summary>
        /// The state information of the graph that the pathfinding is performed on that is constant. This stores obstacles, and tile locations.
        /// </summary>
        public TileGrid tileGrid {
            get
            {
                return tiles;
            }
        }

        /// <summary>
        /// The state information of the graph that the pathfinding is performed on that is constant. This stores obstacles, and tile locations.
        /// </summary>
        protected TileGrid tiles;

        /// <summary>
        /// A rough estimate of the minimum cost to travel from one tile to another
        /// </summary>
        public const float horizontalCost = 1;
        public const float diagonalCost = 1.41421356237309504880f; //Root 2 approx

        //The Dimensions of the grid that the pathfinding is performed on.
        public readonly int length;
        public readonly int width;

        /// <summary>
        /// Where length and width are the dimensions of the grid.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        public AStar(int length, int width)
        {
            tiles = new TileGrid(length, width);
            this.length = length;
            this.width = width;
        }


        /// Loosely based on https://www.redblobgames.com/pathfinding/a-star/introduction.html
        ///From Red Blob Games.
        /// And http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#breaking-ties
        /// 
        ///More detailed breakdown from https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2
        ///From Nicholas W. Swift

        /// <summary>
        /// Uses A* to find a path from the starting location, to the target location.
        /// Returns true if a path was found, which is stored in the out variable of path.
        /// </summary>
        /// <param name="startingLocation"></param>
        /// <param name="targetLocation"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FindPath(Vector2Int startingLocation, Vector2Int targetLocation, out AStarPath path)
        {
            //The AStarTileGrid stores the state information for each node (each tile in the grid)
            AStarTileGrid aStarTileGrid = new AStarTileGrid(length, width, ref tiles);

            //Acts as a priority queue to deterimine which tile to move through next
            List<AStarTile> openTiles = new List<AStarTile>();

            //Add the starting tile to the open list, to start with.
            openTiles.Add(new AStarTile(0, Vector2Int.Distance(startingLocation, targetLocation)));


            AStarTile currentTile = openTiles[0];
            bool pathFound = false;

            //Main loop
            while(openTiles.Count != 0)
            {
                //As openTiles is a sorted list based on the F scores, the first item is tile with the lowest cost.
                currentTile = openTiles[0];

                //If the current tile is at the targetLocation, a path is found and we can terminate our search
                if (currentTile.location == targetLocation)
                {
                    pathFound = true;
                    break;
                }

                //For each adjacent tile that is not an obstacle
                foreach(AStarTile neighbour in aStarTileGrid.GetAdjacentEmptyTiles(currentTile.location))
                {

                    float routeCost = currentTile.G + Vector2Int.Distance(currentTile.location, neighbour.location);

                    //Explore the neighbouring tile, and update it's costs
                    if (!neighbour.hasBeenExplored)
                    {
                        neighbour.G = routeCost;
                        neighbour.H = CalculateH(neighbour.location, targetLocation);
                        neighbour.hasBeenExplored = true;
                        neighbour.parentLocation = currentTile.location;
                        openTiles.Add(neighbour);
                        continue;
                    }


                    //Neighbour has been explored. Have we found a short root to this tile?
                    if(routeCost < neighbour.G)
                    {
                        //A new path was found.
                        neighbour.G = routeCost;
                        neighbour.parentLocation = currentTile.location;

                        //Reopen the tile if necessary
                        if (!openTiles.Contains(neighbour))
                        {
                            openTiles.Add(neighbour);
                        }
                    }
                    


                }

                //Remove the tile from the open list, so that the A* examines the next tile. Required for termination of the main loop.
                openTiles.Remove(currentTile);

                //TODO: Replace this with a more optimized method of updating the priority queue. Note that using a lambda here is not efficient.
                openTiles.Sort((x, y) => x.F.CompareTo(y.F));
            }

            if(pathFound)
            {
                path = BuildPath(ref aStarTileGrid, currentTile, startingLocation);
                return true;
            }

            //Unable to find a path
            path = null; 
            return false;

           

        }

        //Octile Distance heuristic based on  
        // http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#breaking-ties
        //
        
        /// <summary>
        /// Octile Distance Heuristic Function
        /// </summary>
        /// <param name="gridLocation"></param>
        /// <param name="endLocation"></param>
        /// <returns></returns>
       
        private float CalculateH(Vector2Int gridLocation, Vector2Int endLocation)
        {
            float dx = Mathf.Abs(gridLocation.x - endLocation.x);
            float dy = Mathf.Abs(gridLocation.y - endLocation.y);
            return horizontalCost * (dx + dy) + (diagonalCost - 2 * horizontalCost) * Mathf.Min(dx, dy);
        }

        /// <summary>
        /// Assembles a path from the data stored in the AStarTileGrid, tracing the tiles using each tile's parent location.
        /// </summary>
        /// <param name="aStarTileGrid"></param>
        /// <param name="endTile"></param>
        /// <param name="startingLocation"></param>
        /// <returns></returns>
        private AStarPath BuildPath(ref AStarTileGrid aStarTileGrid, AStarTile endTile, Vector2Int startingLocation)
        {
            AStarTile priorTile = endTile;
            AStarPath path = new AStarPath();

            //Trace through each parent, as the path is effectively stored as a linked list, where the endTile is the start of the list.
            path.AddTile(tiles[endTile.location]);
            while(priorTile.location != startingLocation)
            {
                path.AddTile(tiles[priorTile.location]);
                priorTile = aStarTileGrid[priorTile.parentLocation];

            }

            //The starting location is not added in the above loop, so we manually add it after 
            path.AddTile(tiles[startingLocation]);
            return path;
        }


       
    }
   
    


}
