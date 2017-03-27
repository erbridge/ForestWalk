using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float RotationSpeed = 1f;

    public float MinFov = 45;
    public float NormalFov = 60;
    public float MaxFov = 90;

    private Camera _camera;

    private Vector3    _offset;
    private Quaternion _rotation = Quaternion.identity;

    private float _rotationXAxis = 0;
    private float _rotationYAxis = 0;

    private FovStates _fovState = FovStates.Reset;

    private enum FovStates {
        PullIn,
        Reset,
        PullOut

    }

    void Start() {
        this._camera = this.GetComponent<Camera>();

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

    public void PullOutFov() {
        if (this._fovState == FovStates.PullOut) {
            return;
        }

        this._fovState = FovStates.PullOut;

        this.StopAllCoroutines();

        this.StartCoroutine(this.AdjustFov(this.MaxFov, 60));
    }

    public void ResetFov() {
        if (this._fovState == FovStates.Reset) {
            return;
        }

        this._fovState = FovStates.Reset;

        this.StopAllCoroutines();

        this.StartCoroutine(this.AdjustFov(this.NormalFov, 2));
    }

    public void PullInFov() {
        if (this._fovState == FovStates.PullIn) {
            return;
        }

        this._fovState = FovStates.PullIn;

        this.StopAllCoroutines();

        this.StartCoroutine(this.AdjustFov(this.MinFov, 1));
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

    private IEnumerator AdjustFov(float targetFov, float duration) {
        float initialFov = this._camera.fieldOfView;
        float t = 0;

        do {
            t += Time.deltaTime / duration;

            this._camera.fieldOfView = Mathf.Lerp(initialFov, targetFov, t);

            yield return null;
        } while (t < 1);
    }

}
