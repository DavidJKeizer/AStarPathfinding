using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding
{

    [RequireComponent(typeof(RawImage))]
    public class ImagePathfinder : MonoBehaviour
    {

        public Vector2Int startingLocation;
        public Vector2Int destinationLocation;

        public bool recalcPath = false;
        public int numberOfObstacles = 10;



        private AStar aStar;
        private int width;
        private int height;
        private RawImage rawImage;
        private RectTransform rectTransform;

        private Texture2D _texture2D;
        private Texture2D texture2D
        {
            get
            {
                return _texture2D;
            }
            set
            {
                _texture2D = value;
                rawImage.texture = value;
            }
        }


        protected void Awake()
        {
            rawImage = gameObject.GetComponent<RawImage>();
            rectTransform = gameObject.GetComponent<RectTransform>();
        }


        // Start is called before the first frame update
        protected void Start()
        {

            width = Mathf.RoundToInt(rectTransform.rect.width);
            height = Mathf.RoundToInt(rectTransform.rect.height);

            aStar = new AStar(width, height);
            aStar.tileGrid.GenerateObstacles(numberOfObstacles);

            texture2D = new Texture2D(width, height);

            DisplayTileGrid();
        }


        public void RecalculatePath()
        {
            DisplayTileGrid();
            AStarPath path;
            if(aStar.FindPath(startingLocation,destinationLocation, out path))
            {
                ShowPath(path);
            }
            
        }

        private void DisplayTileGrid()
        {
            aStar.tileGrid.ReadForEachItem(MarkObstacle);
            texture2D.Apply();
        }


        private void MarkObstacle(Tile tile)
        {

            texture2D.SetPixel(tile.Location.x, tile.Location.y, tile.isObstacle ? Color.black : Color.white);
        }

        public void ShowPath(AStarPath path)
        {
            Tile nextTile;
            while(path.GetNextTile(out nextTile)){
                texture2D.SetPixel(nextTile.Location.x, nextTile.Location.y, Color.blue);
            }
            texture2D.Apply();
        }



        public Texture2D GetTexture()
        {
            return texture2D;
        }

        public void Update()
        {
            if(recalcPath)
            {
                RecalculatePath();
                recalcPath = false;
            }
        }
    }

}
