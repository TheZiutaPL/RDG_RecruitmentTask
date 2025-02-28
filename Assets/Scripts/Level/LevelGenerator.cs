using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    private const int NOISE_ORIGIN_MAX = short.MaxValue * 4;

    [SerializeField] private Vector2Int mapSize;
    private static Vector2Int halfSizeOffset;

    private static bool[,] groundMask;

    /// <summary>
    /// Checks if this world position is on a solid ground
    /// </summary>
    public static bool IsGround(Vector2 position)
    {
        if (groundMask == null)
            return false;

        Vector2Int roundedPosition = Vector2Int.RoundToInt(position) + halfSizeOffset;
        if (roundedPosition.x < 0 || roundedPosition.x >= groundMask.GetLength(0) || roundedPosition.y < 0 || roundedPosition.y >= groundMask.GetLength(1))
            return false;

        return groundMask[roundedPosition.x, roundedPosition.y];
    }

    [Header("Visuals")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private RuleTile sandTile;
    [SerializeField] private RuleTile grassTile;
    [Space(5)]
    [SerializeField] private Tilemap clutterTilemap;
    [SerializeField] private Tilemap obstacleTilemap;
    [SerializeField] private RuleTile groundClutterTile;
    [SerializeField] private RuleTile groundObstacleTile;
    [SerializeField] private RuleTile waterClutterTile;
    [SerializeField] private RuleTile waterObstacleTile;

    [Header("Randomness")]
    [SerializeField] private bool randomizeSeed;
    [SerializeField] private int seed;

    [Header("Island Generation")]
    [SerializeField] private float islandNoiseScale = 6;
    [SerializeField] private float groundStepCurveRange = 15;
    [SerializeField, Tooltip("Minimum value for placing ground and its changing by distance from (0, 0) position")]
    private AnimationCurve groundStepByDistance;
    [SerializeField, Tooltip("Value added to the groundStep to place grass instead of sand")]
    private float grassAdditionalStep = 0.125f;

    [Header("Extras Generation")]
    [SerializeField] private float obstacleNoiseScale = 10;
    [SerializeField, Range(0, 1)] private float groundExtrasStep = .6f;
    [SerializeField, Range(0, 1)] private float waterExtrasStep = .8f;
    [SerializeField, Range(0, 1)] private float clutterToObstacleRatio = .5f;
    [SerializeField] private float spawnProtectionRange = 7.5f;

    private System.Random randomizer;

    private void Start()
    {        
        GenerateLevel();
    }

    [ContextMenu("Generate Level")]
    public void GenerateLevel()
    {
        halfSizeOffset = mapSize / 2;

        //Randomizes generation seed
        if (randomizeSeed)
            seed = Random.Range(int.MinValue, int.MaxValue);

        //Clears tilemaps from previous tiles
        groundTilemap.ClearAllTiles();
        clutterTilemap.ClearAllTiles();
        obstacleTilemap.ClearAllTiles();

        //Creates randomizer with a specified seed
        randomizer = new System.Random(seed);
        
        GenerateMap();

        GenerateClutterAndObstacles();
    }

    /// <summary>
    /// Generates rough shape of a map (grass, sand)
    /// </summary>
    private void GenerateMap()
    {
        //Makes a 2d bool determining what is ground
        groundMask = new bool[mapSize.x, mapSize.y];

        //Creates map noise
        float[,] noise = GetPerlinNoise(mapSize.x, mapSize.y, islandNoiseScale);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                //Calculates tile position and its distance from (0, 0)
                Vector3Int tilePosition = new Vector3Int(x - halfSizeOffset.x, y - halfSizeOffset.y);
                float distanceFromStart = Vector3Int.Distance(tilePosition, Vector3Int.zero);

                //Gets groundStep value for this tile and sets it
                float groundStep = groundStepByDistance.Evaluate(distanceFromStart / groundStepCurveRange);

                float tileNoise = noise[x, y];
                bool containsGround = tileNoise >= groundStep;
                groundMask[x, y] = containsGround;

                if (!containsGround)
                    continue;

                //Sets sand tile
                groundTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, -1), sandTile);

                //Sets grass tile
                if (tileNoise - groundStep >= grassAdditionalStep)
                    groundTilemap.SetTile(tilePosition, grassTile);
            }
        }
    }

    /// <summary>
    /// Generates obstacles on map
    /// </summary>
    private void GenerateClutterAndObstacles()
    {
        float[,] noise = GetPerlinNoise(mapSize.x, mapSize.y, obstacleNoiseScale);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x - halfSizeOffset.x, y - halfSizeOffset.y);
                float distanceFromStart = Vector3Int.Distance(tilePosition, Vector3Int.zero);

                bool isOnGround = groundMask[x, y];
                if (noise[x, y] >= (isOnGround ? groundExtrasStep : waterExtrasStep))
                {
                    //Decides if clutter or obstacle is placed
                    double rand = randomizer.NextDouble();
                    if(rand < clutterToObstacleRatio || distanceFromStart < spawnProtectionRange)
                        clutterTilemap.SetTile(tilePosition, isOnGround ? groundClutterTile : waterClutterTile);
                    else
                        obstacleTilemap.SetTile(tilePosition, isOnGround ? groundObstacleTile : waterObstacleTile);
                }
            }
        }
    }

    public float[,] GetPerlinNoise(int width, int height, float scale = 1)
    {
        float[,] noise = new float[width, height];

        //Gets a random starting point for noise
        int xOrg = randomizer.Next(NOISE_ORIGIN_MAX);
        int yOrg = randomizer.Next(NOISE_ORIGIN_MAX);
        Debug.Log($"Noise origin at ({xOrg}, {yOrg})");

        //Reads Perlin Noise for each position
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = xOrg + (float)x / width * scale;
                float yCoord = yOrg + (float)y / height * scale;

                //It needs to be clamped because in rare cases it may be below zero
                noise[x, y] = Mathf.Clamp01(Mathf.PerlinNoise(xCoord, yCoord));
            }
        }

        return noise;
    }
}
