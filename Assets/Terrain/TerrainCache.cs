using System.Collections.Generic;
using System.Linq;

public class TerrainCache {

    private Dictionary<Vector2i, TerrainChunk> _loadedChunks;

    public TerrainCache() {
        this._loadedChunks = new Dictionary<Vector2i, TerrainChunk>();
    }

    public void AddChunk(TerrainChunk chunk) {
        this._loadedChunks.Add(chunk.Position, chunk);
    }

    public void RemoveChunk(int x, int z) {
        this._loadedChunks.Remove(new Vector2i(x, z));
    }

    public TerrainChunk GetChunk(Vector2i chunkPosition) {
        if (this._loadedChunks.ContainsKey(chunkPosition)) {
            return this._loadedChunks[chunkPosition];
        }

        return null;
    }

    public List<Vector2i> GetChunks() {
        return this._loadedChunks.Keys.ToList();
    }

}
