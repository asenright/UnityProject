using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public TerrainType(string name, float height, Color color)
    {
        this.name = name;
        this.height = height;
        this.color = color;

    }
    public string name;
    public float height;
    public Color color;
}


public class TileGeneration : MonoBehaviour {
    public enum VisualizationMode { Height, Heat }

    [SerializeField]
	public NoiseMapGeneration noiseMapGeneration;

	[SerializeField]
	public MeshRenderer tileRenderer;

	[SerializeField]
    public MeshFilter meshFilter;

	[SerializeField]
    public MeshCollider meshCollider;

	[SerializeField]
    public float levelScale = 1;

	[SerializeField]
    public TerrainType[] heightTerrainTypes = new TerrainType[] { new TerrainType("default", 0, Color.red) };

    [SerializeField]
    public TerrainType[] heatTerrainTypes = new TerrainType[] { new TerrainType("default", 0, Color.red) };

    [SerializeField]
    public float heightMultiplier;

	[SerializeField]
    public AnimationCurve heightCurve;
    [SerializeField]
    public AnimationCurve heatCurve;

    [SerializeField]
    public Wave[] heightWaves;

    [SerializeField]
    public Wave[] heatWaves;

    [SerializeField]
    public long seed;

    [SerializeField]
    public VisualizationMode visualizationMode;





	//void Start() {
	//	//GenerateTile ();
	//}

    //public void GenerateTile(float centerVertexZ, float maxDistanceZ) {
    //	// calculate tile depth and width based on the mesh vertices
    //	Vector3[] meshVertices = this.meshFilter.mesh.vertices;
    //	int tileDepth = (int)Mathf.Sqrt (meshVertices.Length);
    //	int tileWidth = tileDepth;

    //	// calculate the offsets based on the tile position
    //	float offsetX = -this.gameObject.transform.position.x;
    //	float offsetZ = -this.gameObject.transform.position.z;

    //       // generate a heightMap using noise
    //       float[,] heightMap = this.noiseMapGeneration.GenerateSimplexNoiseMap(tileDepth, tileWidth, this.levelScale, offsetX, offsetZ, seed, waves);


    //       // calculate vertex offset based on the Tile position and the distance between vertices
    //       Vector3 tileDimensions = this.meshFilter.mesh.bounds.size;
    //       float distanceBetweenVertices = tileDimensions.z / (float)tileDepth;
    //       float vertexOffsetZ = this.gameObject.transform.position.z / distanceBetweenVertices;

    //       // generate a heatMap using uniform noise
    //       float[,] heatMap = this.noiseMapGeneration.GenerateUniformNoiseMap(tileDepth, tileWidth, centerVertexZ, maxDistanceZ, vertexOffsetZ);


    //       // build a Texture2D from the height map
    //       Texture2D tileTexture = BuildTexture (heightMap);
    //	this.tileRenderer.material.mainTexture = tileTexture;

    //	// update the tile mesh vertices according to the height map
    //	UpdateMeshVertices (heightMap);
    //   }

    public void GenerateTile(float centerVertexZ, float maxDistanceZ)
    {
        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;

        // calculate the offsets based on the tile position
        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;

        // generate a heightMap using Perlin Noise
        float[,] heightMap = this.noiseMapGeneration.GenerateSimplexNoiseMap(tileDepth, tileWidth, this.levelScale, offsetX, offsetZ, seed, this.heightWaves);

        // calculate vertex offset based on the Tile position and the distance between vertices
        Vector3 tileDimensions = this.meshFilter.mesh.bounds.size;
        float distanceBetweenVertices = tileDimensions.z / (float)tileDepth;
        float vertexOffsetZ = this.gameObject.transform.position.z / distanceBetweenVertices;

        // generate a heatMap using uniform noise
        float[,] uniformHeatMap = this.noiseMapGeneration.GenerateUniformNoiseMap(tileDepth, tileWidth, centerVertexZ, maxDistanceZ, vertexOffsetZ);
        float[,] randomHeatMap = this.noiseMapGeneration.GenerateSimplexNoiseMap(tileDepth, tileWidth, this.levelScale, offsetX, offsetZ, seed, this.heatWaves);
        float[,] heatMap = new float[tileDepth, tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                heatMap[zIndex, xIndex] = uniformHeatMap[zIndex, xIndex] * randomHeatMap[zIndex, xIndex];
                heatMap[zIndex, xIndex] += this.heatCurve.Evaluate(heightMap[zIndex, xIndex]) * heightMap[zIndex, xIndex];
            }
        }

        // build a Texture2D from the height map
        Texture2D heightTexture = BuildTexture(heightMap, this.heightTerrainTypes);
        // build a Texture2D from the heat map
        Texture2D heatTexture = BuildTexture(heatMap, this.heatTerrainTypes);

        switch (this.visualizationMode)
        {
            case VisualizationMode.Height:
                // assign material texture to be the heightTexture
                this.tileRenderer.material.mainTexture = heightTexture;
                break;
            case VisualizationMode.Heat:
                // assign material texture to be the heatTexture
                this.tileRenderer.material.mainTexture = heatTexture;
                break;
        }

        // update the tile mesh vertices according to the height map
        UpdateMeshVertices(heightMap);
    }

    private Texture2D BuildTexture(float[,] heightMap, TerrainType[] terrainTypes)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                // choose a terrain type according to the height value
                TerrainType terrainType = ChooseTerrainType(height, terrainTypes);
                // assign the color according to the terrain type
                colorMap[colorIndex] = terrainType.color;
            }
        }

        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }

    TerrainType ChooseTerrainType(float height, TerrainType[] terrainTypes) {
		// for each terrain type, check if the height is lower than the one for the terrain type
		foreach (TerrainType terrainType in terrainTypes) {
			// return the first terrain type whose height is higher than the generated one
			if (height < terrainType.height) {
				return terrainType;
			}
		}
		return heightTerrainTypes [0];
	}

    private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Vector3[] meshVertices = this.meshFilter.mesh.vertices;

        // iterate through all the heightMap coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];

                Vector3 vertex = meshVertices[vertexIndex];
                // change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);

                vertexIndex++;
            }
        }

        // update the vertices in the mesh and update its properties
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds();
        this.meshFilter.mesh.RecalculateNormals();
        // update the mesh collider
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }
}

