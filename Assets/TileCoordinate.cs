using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class TileCoordinate
    {
        public int tileZIndex;
        public int tileXIndex;
        public int coordinateZIndex;
        public int coordinateXIndex;

        public TileCoordinate(int tileZIndex, int tileXIndex, int coordinateZIndex, int coordinateXIndex)
        {
            this.tileZIndex = tileZIndex;
            this.tileXIndex = tileXIndex;
            this.coordinateZIndex = coordinateZIndex;
            this.coordinateXIndex = coordinateXIndex;
        }
    }
}
