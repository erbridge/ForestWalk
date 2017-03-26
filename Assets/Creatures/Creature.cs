using UnityEngine;

public class Creature : MonoBehaviour {

    public float MoveSpeed = 1f;

    // public TerrainGenerator TerrainGenerator;
    public Animator SpriteAnimator;

    private States _state;

    private enum States {
        Idle,
        Move

    }

    void Start() {
        this._state = States.Idle;

        this.SpriteAnimator.SetBool("IsStill", true);
    }

    void Update() {
        if (this._state != States.Idle) {
            float speed = this.MoveSpeed;

            // FIXME: Make this a random direction.
            this.transform.Translate(
                Camera.main.transform.forward * speed * Time.deltaTime
            );
        }

        // Vector3 position = this.transform.position;
        //
        // position.y = this.TerrainGenerator.GetTerrainHeight(position);
        //
        // this.transform.position = position;
        //
        // this.TerrainGenerator.UpdateTerrain(this.transform.position, 2);
    }

    public void ToggleMovement() {
        if (this._state == States.Idle) {
            this._state = States.Move;

            this.SpriteAnimator.SetBool("IsStill", false);
        } else {
            this._state = States.Idle;

            this.SpriteAnimator.SetBool("IsStill", true);
        }
    }

}
