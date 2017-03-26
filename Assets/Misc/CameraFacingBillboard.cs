using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour {

    private Camera _camera;
    private SpriteRenderer _spriteRenderer;

    void Awake() {
        this._camera = Camera.main;
        this._spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    void Update() {
        this.transform.LookAt(
            this.transform.position + this._camera.transform.rotation *
            Vector3.forward,
            this._camera.transform.rotation * Vector3.up
        );
    }

    void LateUpdate() {
        // this._spriteRenderer.sortingOrder = -1 *
        // (int) this._camera.WorldToScreenPoint(
        //     this._spriteRenderer.bounds.min
        // ).y;
    }

}
