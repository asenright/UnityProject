using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    [SerializeField]
    public int mapWidthInTiles, mapDepthInTiles;

    [SerializeField]
    public GameObject tilePrefab;

    [SerializeField]
    public long seed;

    [SerializeField]
    public float heightMultiplier, scale;
    
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        // for each Tile, instantiate a Tile in the correct position
        for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {
                // calculate the tile position based on the X and Z indices
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
                    this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z + zTileIndex * tileDepth);

                // instantiate a new Tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                var tileGenComponent = tile.GetComponent<TileGeneration>();
                tileGenComponent.seed = seed;
                tileGenComponent.levelScale = scale;
                tileGenComponent.heightMultiplier = heightMultiplier;





            }
        }
    }
}
