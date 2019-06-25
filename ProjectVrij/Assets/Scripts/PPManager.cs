using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPManager : MonoBehaviour
{
    public static PPManager Instance;

    private PostProcessVolume ppVolume;
    private ColorGrading colorGrading;
    private Vignette vignette;

    [SerializeField] private AnimationCurve shiftSaturationCurve;
    [SerializeField] private float shiftSaturationDuration;

    Coroutine shiftSaturationRoutine;

    private void Awake()
    {
        Instance = Instance ?? this;

        ppVolume = GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out colorGrading);
        ppVolume.profile.TryGetSettings(out vignette);
    }

    public void ShiftSaturation(float _value)
    {
        if (shiftSaturationRoutine != null) StopCoroutine(shiftSaturationRoutine);
        shiftSaturationRoutine = StartCoroutine(IShiftSaturation(_value));
    }

    IEnumerator IShiftSaturation(float _endValue)
    {
        float _startingValue = colorGrading.saturation.value;
        float _startVignette = vignette.roundness.value;

        float _lerpTime = 0f;
        while (_lerpTime < 1f)
        {
            _lerpTime += Time.deltaTime / shiftSaturationDuration;
            float _lerpKey = shiftSaturationCurve.Evaluate(_lerpTime);

            colorGrading.saturation.value = Mathf.Lerp(_startingValue, Mathf.Clamp(_endValue,-100,100), _lerpKey);

            //SMERIGE VIGNETTE FIX, VERWIJDER ALS KAPOT IS
            vignette.roundness.value = Mathf.Lerp(_startVignette, -Mathf.Clamp((_endValue + 100) / 100, 0, 1) + 1, _lerpKey);

            yield return null;
        }
        yield return null;
    }
}
