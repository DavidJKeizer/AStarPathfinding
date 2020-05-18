using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pathfinding { 

    
    
    public struct Tile
    {
        public float terrainCost { get; set; }
        public bool isObstacle { get; set; }
        public Vector2Int Location { get; set; }


        public override string ToString()
        {
            return $"Tile {Location} is {(isObstacle ? "an abstacle. " : "not an obstacle")}";
        }

    }


}


