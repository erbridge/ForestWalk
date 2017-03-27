using UnityEngine;

public class Creature : MonoBehaviour {

    public float MoveSpeed  = 1f;
    public float JitterRate = 0.99f;
    public bool  ShouldFly  = false;
    public float MinHeight  = 2f;
    public float MaxHeight  = 20f;

    public TerrainGenerator TerrainGenerator;
    public Animator SpriteAnimator;
    public SpriteRenderer SpriteRenderer;

    private Vector3 _moveDirection;
    private float   _height = 0;

    private States _state;

    private enum States {
        Idle,
        Move

    }

    void Start() {
        if (this.ShouldFly || (Random.value > 0.2f)) {
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

        if (this.ShouldFly) {
            this._height = Random.Range(this.MinHeight, this.MaxHeight);
        }
    }

    void Update() {
        if (!this.ShouldFly && (Random.value > 0.995f)) {
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
                if (Random.value > this.JitterRate) {
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

        Vector3 position = this.transform.position;

        if (this.TerrainGenerator != null) {
            position.y = this.TerrainGenerator.GetTerrainHeight(position);
        }

        if (this.ShouldFly && (this._state != States.Idle)) {
            this._height += Random.Range(-1f, 1f) *
            this.MoveSpeed / 2 * Time.deltaTime;

            this._height = Mathf.Clamp(
                this._height,
                this.MinHeight,
                this.MaxHeight
            );

            position.y += this._height;
        }

        this.transform.position = position;
    } // Update

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
