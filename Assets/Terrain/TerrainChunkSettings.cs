using System.Collections.Generic;
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

    public List<Texture2D> Textures {
        get;

        private set;
    }

    public TerrainChunkSettings(
        int heightmapResolution,
        int alphamapResolution,
        int length,
        int height,
        List<Texture2D> textures
    ) {
        this.HeightmapResolution = heightmapResolution;
        this.AlphamapResolution  = alphamapResolution;
        this.Length   = length;
        this.Height   = height;
        this.Textures = textures;
    }

}
