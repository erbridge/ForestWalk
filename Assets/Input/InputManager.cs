using UnityEngine;

public class InputManager : MonoBehaviour {

    public Character Character;
    public CameraController CameraController;
    public UIManager UIManager;

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (this.UIManager.IsUIVisible()) {
                this.UIManager.HideUI();
            } else {
                this.Character.ToggleMovement();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (this.UIManager.IsUIVisible()) {
                this.UIManager.HideUI();
            } else {
                this.Character.ToggleRun();
            }
        }

        Vector2 mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        if (mouseDelta != Vector2.zero) {
            this.CameraController.UpdateRotation(mouseDelta);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (this.UIManager.IsUIVisible()) {
                Application.Quit();
            } else {
                this.UIManager.ShowUI();
            }
        }
    }

}
