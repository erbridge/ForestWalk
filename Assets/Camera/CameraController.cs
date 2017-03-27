using UnityEngine;

public class CameraController : MonoBehaviour {

    public float RotationSpeed = 2f;

    private Vector3    _offset;
    private Quaternion _rotation = Quaternion.identity;

    private float _rotationXAxis = 0;
    private float _rotationYAxis = 0;

    void Start() {
        this._offset = this.transform.parent.position - this.transform.position;

        this._rotationXAxis = this.transform.eulerAngles.x;
        this._rotationYAxis = this.transform.eulerAngles.y;

        this.UpdateRotation(Vector2.zero);
    }

    void LateUpdate() {
        Vector3   rotatedOffset = this._rotation * this._offset;
        Transform target = this.transform.parent;

        this.transform.position = target.position - rotatedOffset;

        this.transform.LookAt(target);
    }

    public void UpdateRotation(Vector2 mouseDelta) {
        this._rotationXAxis -= mouseDelta.y * this.RotationSpeed;
        this._rotationYAxis += mouseDelta.x * this.RotationSpeed;

        this._rotationXAxis = this.ClampAngle(this._rotationXAxis, -15, 45);

        this._rotation = Quaternion.Euler(
            this._rotationXAxis,
            this._rotationYAxis,
            0
        );
    }

    private float ClampAngle(float angle, float min, float max) {
        if (angle < -360) {
            angle += 360;
        }

        if (angle > 360) {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }

}
