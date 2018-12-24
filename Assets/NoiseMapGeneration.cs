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
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, long seed){//, Wave[] waves) {
        if (scale <= 0) throw new UnityException("GenerateNoiseMap.cs: Scale must be greater than 0");

		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[mapDepth, mapWidth];
        var noisemaker = new OpenSimplexNoise(seed);

        for (int zIndex = 0; zIndex < mapDepth; zIndex++) {
			for (int xIndex = 0; xIndex < mapWidth; xIndex++) {
                // calculate sample indices based on the coordinates, the scale and the offset

                //float sampleX = (xIndex + offsetX) / scale;
                //float sampleZ = (zIndex + offsetZ) / scale;
                float sampleX = (offsetX + xIndex) / (mapWidth) * scale;
                float sampleZ = (offsetZ + zIndex) / (mapDepth) * scale;

                //float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                //noise = randomValue(noise, .1f);
               
                float noise = (float) noisemaker.Evaluate(sampleX, sampleZ);
                noiseMap[zIndex, xIndex] = noise;

                //float normalization = 0f;
                //foreach (Wave wave in waves) {
                //	// generate noise value using PerlinNoise for a given Wave
                //	noise += wave.amplitude * Mathf.PerlinNoise (sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
                //	normalization += wave.amplitude;
                //}
                //// normalize the noise value so that it is within 0 and 1
                //noise /= normalization;

                noiseMap [zIndex, xIndex] = noise;
			}
		}

		return noiseMap;
	}

    /// <summary>
    /// Returns a value between 0 and 1. Sample is the target number, slop is how far from it you can be.
    /// i.e. given sample = .7 and slop = .1, will return a val from .6 to .8
    /// </summary>
    /// <param name="sample"></param>
    /// <param name="slop"></param>
    /// <returns></returns>
    private float randomValue(float sample, float slop)
    {
        float randomVal = (((float)Random.Range(-1000, 1000)) / 1000) * slop;
        return sample + randomVal;
    }
}



[System.Serializable]
public class Wave {
	public float seed;
	public float frequency;
	public float amplitude;
}
