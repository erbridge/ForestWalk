using UnityEngine;

public class InputManager : MonoBehaviour {

    public Character Character;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.Character.ToggleMovement();
        }

        if (Input.GetMouseButtonDown(1)) {
            this.Character.ToggleRun();
        }
    }

}
