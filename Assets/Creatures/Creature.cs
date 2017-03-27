using UnityEngine;

public class Creature : MonoBehaviour {

    public float MoveSpeed = 1f;

    public TerrainGenerator TerrainGenerator;
    public Animator SpriteAnimator;
    public SpriteRenderer SpriteRenderer;

    private Vector3 _moveDirection;

    private States _state;

    private enum States {
        Idle,
        Move

    }

    void Start() {
        if (Random.value > 0.2f) {
            this._state = States.Move;

            this.SpriteAnimator.SetBool("IsStill", false);
        } else {
            this._state = States.Idle;

            this.SpriteAnimator.SetBool("IsStill", true);
        }


        if (Random.value > 0.5f) {
            this._moveDirection = Camera.main.transform.right;
            this.SpriteRenderer.flipX = true;
        } else {
            this._moveDirection = -Camera.main.transform.right;
            this.SpriteRenderer.flipX = false;
        }
    }

    void Update() {
        if (Random.value > 0.995f) {
            if (this._state == States.Idle) {
                this._state = States.Move;

                this.SpriteAnimator.SetBool("IsStill", false);
            } else {
                this._state = States.Idle;

                this.SpriteAnimator.SetBool("IsStill", true);
            }
        }

        if (this._state != States.Idle) {
            float speed = this.MoveSpeed;

            if (Random.value > 0.6f) {
                if (Random.value > 0.99f) {
                    this.SpriteRenderer.flipX = !this.SpriteRenderer.flipX;
                }

                if (this.SpriteRenderer.flipX) {
                    this._moveDirection = Camera.main.transform.right;
                } else {
                    this._moveDirection = -Camera.main.transform.right;
                }
            }

            this.transform.Translate(
                this._moveDirection * speed * Time.deltaTime
            );
        }

        if (this.TerrainGenerator != null) {
            Vector3 position = this.transform.position;

            position.y = this.TerrainGenerator.GetTerrainHeight(position);

            this.transform.position = position;
        }

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
