using UnityEngine;

public class Character : MonoBehaviour {

    public TerrainGenerator TerrainGenerator;

    void Update() {
        this.transform.Translate(Vector3.forward * Time.deltaTime);

        Vector3 position = this.transform.position;

        position.y = this.TerrainGenerator.GetTerrainHeight(position);

        this.transform.position = position;

        this.TerrainGenerator.UpdateTerrain(this.transform.position, 2);
    }

}
