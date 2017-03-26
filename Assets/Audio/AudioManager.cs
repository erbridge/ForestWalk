using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public int HighVolume = 0;
    public int MidVolume  = -6;
    public int LowVolume  = -24;

    public float FocusFadeDuration   = 5f;
    public float UnfocusFadeDuration = 5f;

    public List<AudioClip> MusicClips;

    private List<AudioSource> _musicSources;
    private List<int> _musicSourceFoci;
    private List<Coroutine> _musicSourceFadeCoroutines;

    void Awake() {
        this._musicSources = new List<AudioSource>();

        foreach (AudioClip clip in this.MusicClips) {
            AudioSource source = this.gameObject.AddComponent<AudioSource>();

            source.clip   = clip;
            source.volume = this.ConvertDecibelToLinear(this.MidVolume);
            source.loop   = true;

            source.Play();

            this._musicSources.Add(source);
        }

        this._musicSourceFoci = new List<int>();
        this._musicSourceFadeCoroutines = new List<Coroutine>();
    }

    public void FocusMusic(int count) {
        if (this._musicSourceFoci.Count >= count) {
            return;
        }

        while (this._musicSourceFoci.Count < count) {
            int index;

            do {
                index = UnityEngine.Random.Range(0, this._musicSources.Count);
            } while (this._musicSourceFoci.Contains(index));

            this._musicSourceFoci.Add(index);
        }

        this.StopAllMusicFades();

        for (int i = 0; i < this._musicSources.Count; i++) {
            AudioSource source = this._musicSources[i];
            float targetVolume = this.ConvertDecibelToLinear(this.LowVolume);

            if (this._musicSourceFoci.Contains(i)) {
                targetVolume = this.ConvertDecibelToLinear(this.HighVolume);
            }

            this._musicSourceFadeCoroutines.Add(
                this.StartCoroutine(
                    this.FadeAudio(source, targetVolume, this.FocusFadeDuration)
                )
            );
        }
    }

    public void UnfocusMusic() {
        if (this._musicSourceFoci.Count == 0) {
            return;
        }

        this.StopAllMusicFades();

        foreach (AudioSource source in this._musicSources) {
            this._musicSourceFadeCoroutines.Add(
                this.StartCoroutine(
                    this.FadeAudio(
                        source,
                        this.ConvertDecibelToLinear(this.MidVolume),
                        this.UnfocusFadeDuration
                    )
                )
            );
        }

        this._musicSourceFoci.Clear();
    }

    private IEnumerator FadeAudio(
        AudioSource source,
        float       targetVolume,
        float       duration
    ) {
        float initialVolume = source.volume;

        float sign = Mathf.Sign(targetVolume - initialVolume);

        while (sign * (targetVolume - source.volume) > 0) {
            source.volume += sign * initialVolume * Time.deltaTime / duration;

            yield return null;
        }
    }

    private void StopAllMusicFades() {
        foreach (Coroutine coroutine in this._musicSourceFadeCoroutines) {
            try {
                this.StopCoroutine(coroutine);
            } catch (NullReferenceException) { }
        }

        this._musicSourceFadeCoroutines.Clear();
    }

    private float ConvertDecibelToLinear(int dB) {
        return Mathf.Pow(10f, dB / 20f);
    }

}
