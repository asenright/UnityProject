using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class LevelData
    {
        private int tileDepthInVertices, tileWidthInVertices; //Todo, set these!
        public TileData[,] tileData;

        public TileCoordinate ConvertToTileCoordinate(int zIndex, int xIndex)
        {
            // the tile index is calculated by dividing the index by the number of tiles in that axis
            int tileZIndex = (int)Mathf.Floor((float)zIndex / (float)this.tileDepthInVertices);
            int tileXIndex = (int)Mathf.Floor((float)xIndex / (float)this.tileWidthInVertices);
            // the coordinate index is calculated by getting the remainder of the division above
            // we also need to translate the origin to the bottom left corner
            int coordinateZIndex = this.tileDepthInVertices - (zIndex % this.tileDepthInVertices) - 1;
            int coordinateXIndex = this.tileWidthInVertices - (xIndex % this.tileDepthInVertices) - 1;

            TileCoordinate tileCoordinate = new TileCoordinate(tileZIndex, tileXIndex, coordinateZIndex, coordinateXIndex);
            return tileCoordinate;
        }



        
    }

}
