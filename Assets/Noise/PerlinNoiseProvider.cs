using UnityEngine;

public class PerlinNoiseProvider : INoiseProvider {

    public float GetValue(float x, float z) {
        return Mathf.PerlinNoise(x, z);
    }

}
