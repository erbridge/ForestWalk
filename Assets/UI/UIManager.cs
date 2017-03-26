using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Canvas RootCanvas;

    private bool _isVisible = true;

    public void ShowUI() {
        Image[] images = this.RootCanvas.GetComponentsInChildren<Image>();

        foreach (Image image in images) {
            image.CrossFadeAlpha(1, 1, true);
        }

        this._isVisible = true;
    }

    public void HideUI() {
        Image[] images = this.RootCanvas.GetComponentsInChildren<Image>();

        foreach (Image image in images) {
            image.CrossFadeAlpha(0, 1, true);
        }

        this._isVisible = false;
    }

    public bool IsUIVisible() {
        return this._isVisible;
    }

}
