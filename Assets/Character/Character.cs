using UnityEngine;

public class Character : MonoBehaviour {

    public float WalkSpeed = 0.5f;

    public TerrainGenerator TerrainGenerator;
    public Animator SpriteAnimator;

    void Update() {
        this.SpriteAnimator.SetBool("IsStill", false);

        this.transform.Translate(
            Vector3.forward * this.WalkSpeed * Time.deltaTime
        );

        Vector3 position = this.transform.position;

        position.y = this.TerrainGenerator.GetTerrainHeight(position);

        this.transform.position = position;

        this.TerrainGenerator.UpdateTerrain(this.transform.position, 2);
    }

}
