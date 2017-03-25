using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour {

    private Camera _camera;

    void Awake() {
        this._camera = Camera.main;
    }

    void Update() {
        this.transform.LookAt(
            this.transform.position + this._camera.transform.rotation *
            Vector3.forward,
            this._camera.transform.rotation * Vector3.up
        );
    }

}
