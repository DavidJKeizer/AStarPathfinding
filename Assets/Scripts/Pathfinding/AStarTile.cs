using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pathfinding
{
    public class AStarTile
    {
        private float _G;
        public float G
        {
            get
            {
                return _G;
            }
            set
            {
                _G = value;
                _F = _H + _G;
            }
        }

        private float _H;
        public float H
        {
            get
            {
                return _H;
            }
            set
            {
                _H = value;
                _F = _H + _G;
            }
        }

        private float _F;
        public float F
        {
            get
            {
                return _F;
            }
        }

        public bool hasBeenExplored { get; set; }

        public Vector2Int location { get; set; }
        public Vector2Int parentLocation { get; set; }

        public override string ToString()
        {
            return $"AStar tile {location} has scores of G: {G}, H: {H} = F {F}";
        }

        public AStarTile()
        {
            G = 0;
            H = 0;
            hasBeenExplored = false;
        }

        public AStarTile(float G, float H) : base()
        {
            this.G = G;
            this.H = H;
            hasBeenExplored = true;
        }

    }

}