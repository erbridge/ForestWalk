using UnityEngine;

public class CameraController : MonoBehaviour {

    public float RotationSpeed = 2f;

    private Vector3    _offset;
    private Quaternion _rotation = Quaternion.identity;


    void Start() {
        this._offset = this.transform.parent.position - this.transform.position;
    }

    void LateUpdate() {
        Vector3   rotatedOffset = this._rotation * this._offset;
        Transform target = this.transform.parent;

        this.transform.position = target.position - rotatedOffset;

        this.transform.LookAt(this.transform.parent);
    }

    public void UpdateRotation(Vector2 mouseDelta) {
        this._rotation *= Quaternion.Euler(
            -mouseDelta.y * this.RotationSpeed,
            mouseDelta.x * this.RotationSpeed,
            0
        );
    }

}
