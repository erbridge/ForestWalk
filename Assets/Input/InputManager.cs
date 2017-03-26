using UnityEngine;

public class InputManager : MonoBehaviour {

    public Character Character;
    public CameraController CameraController;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.Character.ToggleMovement();
        }

        if (Input.GetMouseButtonDown(1)) {
            this.Character.ToggleRun();
        }

        Vector2 mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        if (mouseDelta != Vector2.zero) {
            this.CameraController.UpdateRotation(mouseDelta);
        }
    }

}
