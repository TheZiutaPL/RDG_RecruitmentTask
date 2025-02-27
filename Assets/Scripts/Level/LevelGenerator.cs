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
    private Vector2Int halfSizeOffset;

    private bool[,] groundMask;

    /// <summary>
    /// Checks if this world position is on a solid ground
    /// </summary>
    public bool IsGround(Vector2 position)
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
    [SerializeField] private Transform waterTransform;

    [Header("Randomness")]
    [SerializeField] private float noiseScale = 1;
    [SerializeField] private bool randomizeSeed;
    [SerializeField] private int seed;
    
    [SerializeField] private float groundStepCurveRange = 15;
    [SerializeField, Tooltip("Minimum value for placing ground and its changing by distance from (0, 0) position")]
    private AnimationCurve groundStepByDistance;
    [SerializeField, Tooltip("Value detemining if a grass or sand should be placed")]
    private float grassStep = 0.1f;

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

        //Clears tilemap from previous tiles
        groundTilemap.ClearAllTiles();

        //Creates randomizer with a specified seed
        randomizer = new System.Random(seed);
        
        GenerateMap();
    }

    /// <summary>
    /// Generates rough shape of a map (grass, sand)
    /// </summary>
    private void GenerateMap()
    {
        //Makes a 2d bool determining what is ground
        groundMask = new bool[mapSize.x, mapSize.y];

        //Creates map noise
        float[,] noise = GetPerlinNoise(mapSize.x, mapSize.y, noiseScale);

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
                if (tileNoise - groundStep >= grassStep)
                    groundTilemap.SetTile(tilePosition, grassTile);
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
