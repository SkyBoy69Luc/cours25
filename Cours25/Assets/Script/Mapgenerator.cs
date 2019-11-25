using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mapgenerator : MonoBehaviour {

    public enum GenerationType
    {
        RANDOM, PERLINNOISE

    }

    public GenerationType generationType;
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate;
    public int seed;
    public Vector2 offset;
    public Tilemap tilemap;

    public TerrainType[] regions;
    public TerrainType[] minerais;

    public void GenerateMap()
    {
        if (generationType == GenerationType.PERLINNOISE)
        {
            GenateMapWhitNoise();
        }
        else
        {
            GenateMapWhitRandom();
        }
    }

    public void GenateMapWhitNoise()
    {

        float[,] noiseMap  = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        float[,] noiseMap2 = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed +1, noiseScale, octaves, persistance, lacunarity, offset);
        float[,] noiseMap3 = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed +5, noiseScale, octaves, persistance, lacunarity, offset);

        TileBase[] customTilemap = new TileBase[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {

            for (int x = 0; x < mapWidth; x++)
            {
                float rnd = noiseMap[x,y];
                float rnd2 = noiseMap2[x, y];
                float rnd3 = noiseMap2[x, y];
                if (rnd < 0.7)
                {
                    customTilemap[y * mapWidth + x] = FindTileFromRegion(rnd2);

                }
                else 
                {
                    
                    customTilemap[y * mapWidth + x] = FindTileFromMinerais(rnd3);
                }

               
            }

        }
        SetTileMap(customTilemap);
    }

    public void GenateMapWhitRandom()
    {

        TileBase[] customTilemap = new TileBase[mapWidth * mapWidth];
        for (int y = 0; y < mapHeight; y++)
        {

            for (int x = 0; x < mapWidth; x++)
            {
                float rnd = Random.Range(0f, 1f);
                customTilemap[y * mapWidth + x] = FindTileFromRegion(rnd);


            }

        }
        SetTileMap(customTilemap);
    }

    private TileBase FindTileFromRegion(float rnd)
    {
        for (int i = 0; i < regions.Length; i++)
        {
            if (rnd <= regions[i].height)
            {
                return regions[i].tile;
            }
        }
        return regions[0].tile;

    }

    private TileBase FindTileFromMinerais(float rnd)
    {
        for (int i = 0; i < minerais.Length; i++)
        {
            if (rnd <= minerais[i].height)
            {
                return minerais[i].tile;
            }
        }
        return minerais[0].tile;

    }

    private void SetTileMap(TileBase[] customTilemap)
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), customTilemap[y * mapWidth + x]);
            }
        }
    }

    private void OnValidate()
    {
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }

        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }

    }
}



[System.Serializable]

public struct TerrainType
{
    public string name;
    public float height;
    public TileBase tile;
}
