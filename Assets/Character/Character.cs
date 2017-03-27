using UnityEngine;

public class Character : MonoBehaviour {

    public float WalkSpeed = 0.5f;
    public float RunSpeed  = 2f;

    public AudioManager AudioManager;
    public TerrainGenerator TerrainGenerator;
    public CameraController CameraController;
    public Animator SpriteAnimator;

    private States _state;

    private enum States {
        Idle,
        Walk,
        Run

    }

    void Start() {
        this._state = States.Walk;

        this.SpriteAnimator.SetBool("IsStill", false);
        this.SpriteAnimator.SetBool("IsRunning", false);
    }

    void Update() {
        float speed = 0f;

        if (this._state == States.Walk) {
            speed = this.WalkSpeed;
        } else if (this._state == States.Run) {
            speed = this.RunSpeed;
        }

        if (speed > 0) {
            this.transform.Translate(
                Camera.main.transform.forward * speed * Time.deltaTime
            );
        }

        Vector3 position = this.transform.position;

        position.y = this.TerrainGenerator.GetTerrainHeight(position);

        this.transform.position = position;

        this.TerrainGenerator.UpdateTerrain(this.transform.position, 2);
    }

    public void ToggleMovement() {
        if (this._state == States.Idle) {
            this._state = States.Walk;

            this.SpriteAnimator.SetBool("IsStill", false);

            this.AudioManager.UnfocusMusic();

            this.CameraController.ResetFov();
        } else {
            this._state = States.Idle;

            this.SpriteAnimator.SetBool("IsStill", true);

            this.AudioManager.FocusMusic(2);

            this.CameraController.PullOutFov();
        }

        this.SpriteAnimator.SetBool("IsRunning", false);
    }

    public void ToggleRun() {
        if (this._state == States.Run) {
            this._state = States.Walk;

            this.SpriteAnimator.SetBool("IsRunning", false);

            this.CameraController.ResetFov();
        } else {
            this._state = States.Run;

            this.SpriteAnimator.SetBool("IsRunning", true);

            this.CameraController.PullInFov();
        }

        this.SpriteAnimator.SetBool("IsStill", false);

        this.AudioManager.UnfocusMusic();
    }

}
