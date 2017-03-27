using UnityEngine;

public class TerrainChunk {

    public Vector2i Position {
        get;

        private set;
    }

    public Terrain Terrain {
        get;

        private set;
    }

    public TerrainChunkSettings Settings {
        get;

        private set;
    }

    public INoiseProvider NoiseProvider {
        get;

        private set;
    }

    public TerrainChunk(
        TerrainChunkSettings settings,
        INoiseProvider       noiseProvider,
        int                  x,
        int                  z
    ) {
        this.Settings = settings;
        this.NoiseProvider = noiseProvider;
        this.Position = new Vector2i(x, z);
    }

    public void CreateTerrain() {
        var terrainData = new TerrainData();

        terrainData.heightmapResolution = this.Settings.HeightmapResolution;
        terrainData.alphamapResolution  = this.Settings.AlphamapResolution;

        var heightmap = this.GenerateHeightmap();

        terrainData.SetHeights(0, 0, heightmap);
        terrainData.size = new Vector3(
            this.Settings.Length,
            this.Settings.Height,
            this.Settings.Length
        );

        this.ApplyTextures(terrainData);

        var newTerrainGameObject = Terrain.CreateTerrainGameObject(terrainData);

        newTerrainGameObject.transform.position = new Vector3(
            this.Position.x * this.Settings.Length,
            0,
            this.Position.z * this.Settings.Length
        );
        this.Terrain = newTerrainGameObject.GetComponent<Terrain>();
        this.Terrain.drawTreesAndFoliage = false;
        this.Terrain.materialType = Terrain.MaterialType.BuiltInLegacyDiffuse;
        this.Terrain.Flush();
    }

    public void PopulateTerrain(
        GameObject prefab,
        float      frequency,
        int        resolution
    ) {
        for (float zRes = 0; zRes < resolution; zRes++) {
            for (float xRes = 0; xRes < resolution; xRes++) {
                float xCoordinate = this.Position.x + xRes / (resolution - 1);
                float zCoordinate = this.Position.z + zRes / (resolution - 1);

                if (Random.value > frequency) {
                    Vector3 position = new Vector3(
                        xCoordinate * (resolution - 1),
                        0,
                        zCoordinate * (resolution - 1)
                    );

                    position.y = this.GetTerrainHeight(position);

                    GameObject.Instantiate(
                        prefab,
                        position,
                        Quaternion.identity,
                        this.Terrain.transform
                    );
                }
            }
        }
    }

    public float GetTerrainHeight(Vector3 worldPosition) {
        return this.Terrain.SampleHeight(worldPosition);
    }

    private float[,] GenerateHeightmap() {
        float[,] heightmap = new float[
            this.Settings.HeightmapResolution,
            this.Settings.HeightmapResolution
            ];

        for (
            int zRes = 0;
            zRes < this.Settings.HeightmapResolution;
            zRes++
        ) {
            for (
                int xRes = 0;
                xRes < this.Settings.HeightmapResolution;
                xRes++
            ) {
                float xCoordinate = this.Position.x +
                (float) xRes / (this.Settings.HeightmapResolution - 1);
                float zCoordinate = this.Position.z +
                (float) zRes / (this.Settings.HeightmapResolution - 1);

                heightmap[zRes, xRes] = this.NoiseProvider.GetValue(
                    xCoordinate,
                    zCoordinate
                );
            }
        }

        return heightmap;
    }

    private void ApplyTextures(TerrainData terrainData) {
        var flatSplat  = new SplatPrototype();
        var steepSplat = new SplatPrototype();

        flatSplat.texture  = this.Settings.Texture;
        steepSplat.texture = this.Settings.Texture;

        terrainData.splatPrototypes = new SplatPrototype[]
        {
            flatSplat,
            steepSplat
        };

        terrainData.RefreshPrototypes();

        float[,,] splatMap = new float[
            terrainData.alphamapResolution,
            terrainData.alphamapResolution,
            2
        ];

        for (int zRes = 0; zRes < terrainData.alphamapHeight; zRes++) {
            for (int xRes = 0; xRes < terrainData.alphamapWidth; xRes++) {
                float normalizedX = (float) xRes / (terrainData.alphamapWidth -
                1);
                float normalizedZ = (float) zRes / (terrainData.alphamapHeight -
                1);

                float steepness = terrainData.GetSteepness(
                    normalizedX,
                    normalizedZ
                );
                float steepnessNormalized = Mathf.Clamp(
                    steepness / 1.5f,
                    0,
                    1f
                );

                splatMap[zRes, xRes, 0] = 1f - steepnessNormalized;
                splatMap[zRes, xRes, 1] = steepnessNormalized;
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMap);
    }  // ApplyTextures

}
