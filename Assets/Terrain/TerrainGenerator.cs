using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    public Texture2D Texture;
    public GameObject TreePrefab;
    public GameObject RareTreePrefab;
    public GameObject BushPrefab;
    public GameObject FlowerPrefab;
    public List<GameObject> CreaturePrefabs;

    private INoiseProvider _noiseProvider;
    private TerrainChunkSettings _settings;
    private TerrainCache _cache;

    void Awake() {
        this._noiseProvider = new PerlinNoiseProvider();
        this._settings = new TerrainChunkSettings(
            129,
            129,
            100,
            40,
            this.Texture
        );
        this._cache    = new TerrainCache();
    }

    public void UpdateTerrain(Vector3 worldPosition, int radius) {
        Vector2i chunkPosition = this.GetChunkPosition(worldPosition);

        List<Vector2i> newPositions = this.GetChunkPositionsInRadius(
            chunkPosition,
            radius
        );

        List<Vector2i> loadedChunks = this._cache.GetChunks();

        List<Vector2i> positionsToGenerate = newPositions.Except(
            loadedChunks
        ).ToList();

        foreach (Vector2i position in positionsToGenerate) {
            this.GenerateChunk(position.x, position.z);
        }

        List<Vector2i> chunksToRemove = loadedChunks.Except(
            newPositions
        ).ToList();

        foreach (Vector2i position in chunksToRemove) {
            this.RemoveChunk(position.x, position.z);
        }
    }

    public float GetTerrainHeight(Vector3 worldPosition) {
        Vector2i chunkPosition = this.GetChunkPosition(worldPosition);

        if (chunkPosition != null) {
            TerrainChunk chunk = this._cache.GetChunk(chunkPosition);

            if (chunk != null) {
                return chunk.GetTerrainHeight(worldPosition);
            }
        }

        return 0;
    }

    private void GenerateChunk(int x, int z) {
        TerrainChunk chunk = new TerrainChunk(
            this._settings,
            this._noiseProvider,
            x,
            z
        );

        chunk.CreateTerrain();

        chunk.PopulateTerrain(this.TreePrefab, 0.99f, 129);
        chunk.PopulateTerrain(this.RareTreePrefab, 0.9995f, 129);
        chunk.PopulateTerrain(this.BushPrefab, 0.999f, 129);
        chunk.PopulateTerrain(this.FlowerPrefab, 0.99f, 129);

        foreach (GameObject prefab in this.CreaturePrefabs) {
            chunk.PopulateTerrain(prefab, 0.9995f, 129);
        }

        this._cache.AddChunk(chunk);
    }

    private void RemoveChunk(int x, int z) {
        this._cache.RemoveChunk(x, z);
    }

    private Vector2i GetChunkPosition(Vector3 worldPosition) {
        int x = (int) Mathf.Floor(worldPosition.x / this._settings.Length);
        int z = (int) Mathf.Floor(worldPosition.z / this._settings.Length);

        return new Vector2i(x, z);
    }

    private List<Vector2i> GetChunkPositionsInRadius(
        Vector2i chunkPosition,
        int      radius
    ) {
        var result = new List<Vector2i>();

        for (int zCircle = -radius; zCircle <= radius; zCircle++) {
            for (int xCircle = -radius; xCircle <= radius; xCircle++) {
                if (xCircle * xCircle + zCircle * zCircle < radius * radius) {
                    result.Add(
                        new Vector2i(
                            chunkPosition.x + xCircle,
                            chunkPosition.z + zCircle
                        )
                    );
                }
            }
        }

        return result;
    }

}
