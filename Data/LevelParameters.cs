using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    public class LevelParameters : MonoBehaviour
    {

        [SerializeField]
        public long seed;

        [SerializeField]
        public int width, height;

        [SerializeField]
        public float scale, heightMultiplier;

        public LevelParameters(long seed, int width, int height, float scale, float heightMultiplier)
        {
            this.seed = seed;
            this.width = width;
            this.height = height;
            this.scale = scale;
            this.heightMultiplier = heightMultiplier;
        }
    }
}
