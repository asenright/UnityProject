using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour {

    /// <summary>
    /// Generates a 2D noise map.
    /// </summary>
    /// <param name="mapDepth"></param>
    /// <param name="mapWidth"></param>
    /// <param name="scale"></param>
    /// <param name="offsetX"></param>
    /// <param name="offsetZ"></param>
    /// <returns></returns>
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, long seed, Wave[] waves) {
        if (scale <= 0) throw new UnityException("GenerateNoiseMap.cs: Scale must be greater than 0");

		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[mapDepth, mapWidth];
        var noisemaker = new OpenSimplexNoise(seed);

        for (int zIndex = 0; zIndex < mapDepth; zIndex++) {
			for (int xIndex = 0; xIndex < mapWidth; xIndex++) {
                // calculate sample indices based on the coordinates, the scale and the offset

                //float sampleX = (xIndex + offsetX) / scale;
                //float sampleZ = (zIndex + offsetZ) / scale;
                float sampleX = (offsetX + xIndex) / (mapWidth * scale);
                float sampleZ = (offsetZ + zIndex) / (mapDepth * scale);
                
                //noiseMap[zIndex, xIndex] = noise;
                float normalization = 0f, noise = 0f;
                foreach (Wave wave in waves)
                {
                    // generate noise value using PerlinNoise for a given Wave
                    noise += wave.amplitude * (float)noisemaker.Evaluate(sampleX, sampleZ);
                    normalization += wave.amplitude;
                }
                // normalize the noise value so that it is within 0 and 1
                noise /= normalization;

                noiseMap [zIndex, xIndex] = noise;
			}
		}

		return noiseMap;
	}
}



[System.Serializable]
public class Wave {
	public float seed;
	public float frequency;
	public float amplitude;
}
