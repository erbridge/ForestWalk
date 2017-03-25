using UnityEngine;

public class TerrainChunkSettings {

    public int HeightmapResolution {
        get;

        private set;
    }

    public int AlphamapResolution {
        get;

        private set;
    }

    public int Length {
        get;

        private set;
    }

    public int Height {
        get;

        private set;
    }

    public Texture2D Texture {
        get;

        private set;
    }

    public TerrainChunkSettings(
        int heightmapResolution,
        int alphamapResolution,
        int length,
        int height,
        Texture2D texture
    ) {
        this.HeightmapResolution = heightmapResolution;
        this.AlphamapResolution  = alphamapResolution;
        this.Length  = length;
        this.Height  = height;
        this.Texture = texture;
    }

}
