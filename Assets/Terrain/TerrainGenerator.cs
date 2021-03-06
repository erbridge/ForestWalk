using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    public List<Texture2D> Textures;
    public List<GameObject> TreePrefabs;
    public List<GameObject> RareTreePrefabs;
    public List<GameObject> BushPrefabs;
    public List<GameObject> FlowerPrefabs;
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
            this.Textures
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
            this,
            x,
            z
        );

        chunk.CreateTerrain();

        foreach (GameObject prefab in this.TreePrefabs) {
            chunk.PopulateTerrain(prefab, 1f - 0.01f / this.TreePrefabs.Count);
        }

        foreach (GameObject prefab in this.RareTreePrefabs) {
            chunk.PopulateTerrain(
                prefab, 1f - 0.0005f / this.RareTreePrefabs.Count
            );
        }

        foreach (GameObject prefab in this.BushPrefabs) {
            chunk.PopulateTerrain(prefab, 1f - 0.001f / this.BushPrefabs.Count);
        }

        foreach (GameObject prefab in this.FlowerPrefabs) {
            chunk.PopulateTerrain(
                prefab, 1f - 0.05f / this.FlowerPrefabs.Count
            );
        }

        foreach (GameObject prefab in this.CreaturePrefabs) {
            chunk.PopulateTerrain(
                prefab, 1f - 0.001f / this.CreaturePrefabs.Count
            );
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
