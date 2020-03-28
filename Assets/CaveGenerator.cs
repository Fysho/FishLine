using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CaveGenerator : MonoBehaviour
{

    private int[,] terrainMap;
    private int[,] terrainMapContents;

    public bool useRandomSeed;
    public int seed;

    public int width;
    public int height;

    public Tilemap colliderMap;
    public Tilemap backgroundMap;
    public Tile TileSOLID;
    public Tile TileEMPTY;
    public Tile TileUP;
    public Tile TileRIGHT;
    public Tile TileDOWN;
    public Tile TileLEFT;
    public Tile TileGOLD;
    public Tile TileRUBY;
    public Tile TileDIAMOND;

    public GameObject DropDiamond;
    public GameObject DropRuby;
    public GameObject DropGold;

    public GameObject Enemy;


    public GameObject fallingRock;
    System.Random randomGenerator;

    void Start()
    {
        SetUp();
        GenerateCaves(4,0.5f,0.5f, 0.9f, 10.0f);
        UpdateTileMap();
        GenerateFallingRocks();
       // GenerateEnemies();

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
                        if (randomGenerator.NextDouble() < 0.2f )
                        {
                            Instantiate(fallingRock, new Vector3(x - width / 2 + 0.5f, (height - y) - height / 2 + 0.5f, 0), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    public void GenerateEnemies()
    {
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (terrainMap[x, y] == 0)
                {
                    if (terrainMap[x, y + 1] == 1)
                    {
                        if (randomGenerator.NextDouble() < 0.07f)
                        {
                            Instantiate(Enemy, new Vector3(x - width / 2 + 0.5f, (height - y) - height / 2 + 0.5f, 0), Quaternion.identity);
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
        terrainMapContents = new int[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                terrainMap[x, y] = 0;
                terrainMapContents[x, y] = 0;
            }
        }
    }


    private void GenerateCaves(int octaves, float persistance, float lacunarity, float threshold, float scale)
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
                    float sampleX = (x /*- halfWidth*/ + octaveOffsets[i].x) / (scale * 4) * frequency;
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

                terrainMap[x, y] = noiseHeight < threshold ? 1 : 0;
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
                    if (terrainMap[tilex, tiley] >= 1)
                    {
                        if (terrainMapContents[tilex, tiley] > 0)
                        {
                            if (terrainMapContents[tilex, tiley] == 2)
                            {
                                Debug.Log("instantiating d");
                                Instantiate(DropDiamond, new Vector3(tilex - width / 2 + 0.5f, (height - tiley) - height / 2 + 0.5f, 0), Quaternion.identity);

                            }
                            else if (terrainMapContents[tilex, tiley] == 3)
                            {
                                Debug.Log("instantiating r");

                                Instantiate(DropRuby, new Vector3(tilex - width / 2 + 0.5f, (height - tiley) - height / 2 + 0.5f, 0), Quaternion.identity);

                            }
                            else if (terrainMapContents[tilex, tiley] == 4)
                            {
                                Debug.Log("instantiating g");

                                Instantiate(DropGold, new Vector3(tilex - width / 2 + 0.5f, (height - tiley) - height / 2 + 0.5f, 0), Quaternion.identity);

                            }
                        }
                        terrainMap[tilex, tiley] = 0;
                        terrainMapContents[tilex, tilex] = 0;
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
                //backgroundMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileEMPTY);

                if (terrainMap[x, y] == 1)
                {
                    if (x != 0 && x != width - 1 && y != 0 && y != height - 1)
                    {
                        if (terrainMap[x, y - 1] == 0)
                        {
                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileUP);
                            continue;
                        }
                        else if (terrainMap[x + 1, y] == 0)
                        {
                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileRIGHT);
                            continue;

                        }
                        else if (terrainMap[x, y + 1] == 0)
                        {
                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileDOWN);
                            continue;

                        }
                        else if (terrainMap[x - 1, y] == 0)
                        {
                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileLEFT);
                            continue;

                        }
                        double r = randomGenerator.NextDouble();
                        if(r < 0.02)
                        {
                            terrainMapContents[x, y] = 2;
                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileDIAMOND);
                            continue;

                        }
                        else if (r < 0.05)
                        {
                            terrainMapContents[x, y] = 3;

                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileRUBY);
                            continue;

                        }
                        else if (r < 0.1)
                        {
                            terrainMapContents[x, y] = 4;

                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileGOLD);
                            continue;

                        }
                        else
                        {
                            colliderMap.SetTile(new Vector3Int(x - width / 2, -y + height / 2, 0), TileSOLID);
                        }
                    }
                }
            }
        }
    }
}
