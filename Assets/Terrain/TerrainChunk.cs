using UnityEngine;

public class TerrainChunk {

    public int X {
        get;

        private set;
    }

    public int Z {
        get;

        private set;
    }

    public Terrain Terrain {
        get;

        set;
    }

    public TerrainChunkSettings Settings {
        get;

        set;
    }

    public INoiseProvider NoiseProvider {
        get;

        set;
    }

    public TerrainChunk(
        TerrainChunkSettings settings,
        INoiseProvider       noiseProvider,
        int                  x,
        int                  z
    ) {
        this.Settings = settings;
        this.NoiseProvider = noiseProvider;
        this.X = x;
        this.Z = z;
    }

    public void CreateTerrain() {
        var terrainData = new TerrainData();

        terrainData.heightmapResolution = this.Settings.HeightmapResolution;
        terrainData.alphamapResolution  = this.Settings.AlphamapResolution;

        var heightmap = this.GetHeightmap();

        terrainData.SetHeights(0, 0, heightmap);
        terrainData.size = new Vector3(
            this.Settings.Length,
            this.Settings.Height,
            this.Settings.Length
        );

        var newTerrainGameObject = Terrain.CreateTerrainGameObject(terrainData);

        newTerrainGameObject.transform.position = new Vector3(
            this.X * this.Settings.Length,
            0,
            this.Z * this.Settings.Length
        );
        this.Terrain = newTerrainGameObject.GetComponent<Terrain>();
        this.Terrain.Flush();
    }

    private float[,] GetHeightmap() {
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
                float xCoordinate = this.X +
                (float) xRes / (this.Settings.HeightmapResolution - 1);
                float zCoordinate = this.Z +
                (float) zRes / (this.Settings.HeightmapResolution - 1);

                heightmap[zRes, xRes] = this.NoiseProvider.GetValue(
                    xCoordinate,
                    zCoordinate
                );
            }
        }

        return heightmap;
    }

}
