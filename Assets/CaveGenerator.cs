using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CaveGenerator : MonoBehaviour
{

    private int[,] terrainMap;

    public bool useRandomSeed;
    public int seed;

    public int width;
    public int height;

    public Tilemap colliderMap;
    public Tilemap backgroundMap;
    public Tile Rock;
    public Tile Background;
    public GameObject fallingRock;
    System.Random randomGenerator;

    void Start()
    {
        SetUp();
        GenerateCaves(2,1,1, 10.0f);
        UpdateTileMap();
        GenerateFallingRocks();


    }

    private void GenerateFallingRocks()
    {

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if(terrainMap[x,y] == 0)
                {
                    if (terrainMap[x, y - 1] == 1)
                    {
                        if (randomGenerator.NextDouble() < 1.1f )
                        {
                            Instantiate(fallingRock, new Vector3(x - width / 2 + 0.5f, (height - y) - height / 2 + 0.5f, 0), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    private void SetUp()
    {
        if (useRandomSeed) seed = System.DateTime.UtcNow.Second;
        colliderMap.ClearAllTiles();
        backgroundMap.ClearAllTiles();
        terrainMap = new int[width, height];


    }


    private void GenerateCaves(int octaves, float persistance, float lacunarity, float scale)
    {
        randomGenerator = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float amplitude = 1;
        float frequency = 1;
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = randomGenerator.Next(-100000, 100000);// + offset.x;
            float offsetY = randomGenerator.Next(-100000, 100000);// - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
            //Debug.Log(offsetX);

            // maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        for (int y = 0; y < height; y++)
        {

            for (int x = 0; x < width; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x /*- halfWidth*/ + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y /*- halfHeight*/ + octaveOffsets[i].y) / scale * frequency;


                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    // Debug.Log("SX, SY, PV:   " + sampleX + "  " +  sampleY +  "     " + perlinValue);

                    //if (perlinValue < 0)
                    //{
                    //    Debug.Log("perlin is less than 1 " + noiseHeight);
                    //}
                    //perlinValue = perlinValue * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                terrainMap[x, y] = noiseHeight < 1.0f ? 1 : 0;
            }

        }
    }

    public void Explode(float posX, float posY, float radius)
    {

        for(int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                int tilex = (int)(posX + width / 2 + x + 0.5f - radius / 2);
                int tiley = (int)(-posY + height / 2 + y - 0.5f - radius / 2);
                if(Mathf.Abs(x - radius/2) + Mathf.Abs(y - radius / 2) < radius / 2)
                {
                    if (terrainMap[tilex, tiley] == 1)
                    {
                        terrainMap[tilex, tiley] = 1;
                        colliderMap.SetTile(new Vector3Int(tilex - width / 2, -tiley + height / 2, 0), null);
                    }
                }       
            }
        }
    }

    private void UpdateTileMap()
    {
        colliderMap.ClearAllTiles();
        backgroundMap.ClearAllTiles();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (terrainMap[x, y] == 1)
                {
                    colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), Rock);
                }
                backgroundMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), Background);
            }
        }
    }
}
