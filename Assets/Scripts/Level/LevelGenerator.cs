using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using NavMeshPlus.Components;

public class LevelGenerator : MonoBehaviour
{
    private const int NOISE_ORIGIN_MAX = short.MaxValue * 4;

    [SerializeField] private NavMeshSurface navigationSurface;
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

        Vector2Int roundedPosition = Vector2Int.RoundToInt(position + halfSizeOffset);
        if (roundedPosition.x < 0 || roundedPosition.x >= groundMask.GetLength(0) || roundedPosition.y < 0 || roundedPosition.y >= groundMask.GetLength(1))
            return false;

        return groundMask[roundedPosition.x, roundedPosition.y];
    }

    [Header("Tiles")]
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
    public static int LastGeneratedSeed { get; private set; }

    [Header("Island Generation")]
    [SerializeField] private float islandNoiseScale = 6;
    [SerializeField] private float groundStepCurveRange = 15;
    [SerializeField, Tooltip("Minimum value for placing ground and its changing by distance from (0, 0) position")]
    private AnimationCurve groundStepByDistance;
    [SerializeField, Tooltip("Value added to the groundStep to place grass instead of sand")]
    private float grassAdditionalStep = 0.125f;

    [Space(10)]

    [Header("Extras Generation")]
    [SerializeField] private float obstacleNoiseScale = 10;
    [SerializeField, Range(0, 1)] private float groundExtrasStep = .6f;
    [SerializeField, Range(0, 1)] private float waterExtrasStep = .8f;
    [SerializeField, Range(0, 1)] private float clutterToObstacleRatio = .5f;
    [SerializeField] private float spawnProtectionRange = 7.5f;

    private System.Random randomizer;
    private Collider2D[] spawnCols = new Collider2D[5];

    [Header("Spawning")]
    [SerializeField] private int maxSpawnAttempts = 50;
    [SerializeField] private float spawnCollisionRadius;

    [Header("Coins Spawning")]
    [SerializeField] private Transform coinsParent;
    [SerializeField] private Coin[] coinPrefabs = new Coin[0];
    [SerializeField] private int spawnedCoins;
    [SerializeField, Tooltip("Random value between 0 and 1 is evaluated to give a curve value, higher the value, higher index in array is picked")]
    private AnimationCurve coinsSpawningChanceCurve;
    [SerializeField] private LayerMask coinCollisionMask;

    [Header("Treasure Spawning")]
    [SerializeField] private Transform treasuresParent;
    [SerializeField] private Transform treasureCoinsParent;
    [SerializeField] private Treasure treasurePrefab;
    [SerializeField] private int treasureCount = 3;
    [SerializeField] private int treasureValue = 35;
    [SerializeField] private LayerMask treasureCollisionMask;

    [Header("Enemies Spawning")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private GameObject[] enemyPrefabs = new GameObject[0];
    [SerializeField] private int enemyCount;
    [SerializeField] private LayerMask enemyCollisionMask;

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

        //Clears spawned objects
        ClearTransformChildren(coinsParent);
        ClearTransformChildren(treasuresParent);
        ClearTransformChildren(treasureCoinsParent);
        ClearTransformChildren(enemiesParent);

        //Creates randomizer with a specified seed
        randomizer = new System.Random(seed);
        LastGeneratedSeed = seed;
        
        GenerateMap();

        GenerateClutterAndObstacles();

        SpawnCoinsAndTreasures();

        SpawnEnemies();

        navigationSurface.BuildNavMesh();
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
                groundTilemap.SetTile(tilePosition, sandTile);

                //Sets grass tile
                if (tileNoise - groundStep >= grassAdditionalStep)
                    groundTilemap.SetTile(new Vector3Int(tilePosition.x, tilePosition.y, 1), grassTile);
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

    /// <summary>
    /// Spawns coins and treasures on a map
    /// </summary>
    private void SpawnCoinsAndTreasures()
    {
        //Spawns coins
        if (coinPrefabs.Length > 0)
        {
            //Method that returns desired coin index
            int GetCoinIndex()
            {
                double rand = randomizer.NextDouble();
                float rawIndex = coinsSpawningChanceCurve.Evaluate((float)rand) * coinPrefabs.Length;
                return Mathf.FloorToInt(rawIndex % coinPrefabs.Length);
            }

            //Spawning coins
            for (int i = 0; i < spawnedCoins; i++)
                TrySpawningObjectOnMap(coinPrefabs[GetCoinIndex()], coinsParent, spawnCollisionRadius, coinCollisionMask);
        }

        //Spawns treasures
        List<Transform> treasureTransforms = new List<Transform>();
        for (int i = 0; i < treasureCount; i++)
        {
            Treasure treasure = TrySpawningObjectOnMap(treasurePrefab, treasuresParent, spawnCollisionRadius, treasureCollisionMask);

            if (treasure != null)
            {
                treasure.SetTreasureContent(treasureValue, treasureCoinsParent);
                treasureTransforms.Add(treasure.transform);
            }
        }

        //Sets treasure transforms as player goals
        GameManager.Instance.SetPlayerGoals(treasureTransforms);
    }

    /// <summary>
    /// Spawns enemies on a map
    /// </summary>
    private void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0)
            return;

        //Spawning enemies
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = randomizer.Next(0, enemyPrefabs.Length);
            TrySpawningObjectOnMap(enemyPrefabs[randomIndex], enemiesParent, spawnCollisionRadius, enemyCollisionMask);
        }
    }

    private void ClearTransformChildren(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }

    private T TrySpawningObjectOnMap<T>(T objectToSpawn, Transform parent, float collisionRadius, LayerMask collisionMask) where T : Object
    {
        T spawned = null;

        for (int attempts = 0; attempts < maxSpawnAttempts; attempts++)
        {
            //Gets random position for this attempt
            int x = randomizer.Next(0, mapSize.x);
            int y = randomizer.Next(0, mapSize.y);
            Vector2 position = new Vector2(x, y) - halfSizeOffset;

            //Try spawning on position            
            if (groundMask[x, y] && Physics2D.OverlapCircleNonAlloc(position, collisionRadius, spawnCols, collisionMask) == 0)
            {
                spawned = Instantiate(objectToSpawn, position, Quaternion.identity, parent);
                break;
            }
        }

        return spawned;
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
