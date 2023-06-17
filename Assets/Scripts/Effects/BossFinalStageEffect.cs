using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Events;

namespace Assets.Scripts.Effects
{
    public class BossFinalStageEffect : MonoBehaviour
    {
        public static BossFinalStageEffect Instance { get; private set; }
        [SerializeField] private GameObject ShockWaveObject;

        private ColorAdjustments _colorAdjustments;
        private Vignette _vignette;
        private ChromaticAberration _chromaticAberration;
        private ColorCurves _colorCurves;
        private ShockWaveManager _shockWaveManager;
        private Material _material;

        [field: SerializeField] private Volume Volume { get; set; }
        [field: SerializeField] private float ChromaticAberrationValue { get; set; }
        [field: SerializeField] private float ChromaticAberrationTime { get; set; }

        public UnityEvent hasFinishedEffects;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            _shockWaveManager = GetComponent<ShockWaveManager>();
        }

        private void Start()
        {
            Volume.profile.TryGet(out _colorAdjustments);
            Volume.profile.TryGet(out _vignette);
            Volume.profile.TryGet(out _chromaticAberration);
            Volume.profile.TryGet(out _colorCurves);
            _material = ShockWaveObject.GetComponent<SpriteRenderer>().material;
        }

        public void StartFinalBossStageEffect()
        {
            ShockWaveObject.SetActive(true);
            _shockWaveManager.CallShockWave(_material);
            _colorCurves.active = true;
            StartCoroutine(LerpChromaticAberration(ChromaticAberrationValue, ChromaticAberrationTime));
        }

        private IEnumerator LerpChromaticAberration(float endValue, float duration)
        {
            float time = 0;
            float startValue = _chromaticAberration.intensity.value;
            while (time < duration)
            {
                _chromaticAberration.intensity.value = Mathf.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            while (time > 0)
            {
                _chromaticAberration.intensity.value = Mathf.Lerp(startValue, endValue, time / duration);
                time -= Time.deltaTime;
                yield return null;
            }
            _chromaticAberration.intensity.value = endValue;
            _colorCurves.active = false;
            _colorAdjustments.active = true;
            _vignette.active = true;

            hasFinishedEffects?.Invoke();
            hasFinishedEffects.RemoveAllListeners();
            _chromaticAberration.intensity.value = 0;
            ShockWaveObject.SetActive(false);
        }
    }
}