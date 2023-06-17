using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShockWaveManager : MonoBehaviour
{
    [SerializeField] private float _shockWaveTime = 0.75f;
    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    public void CallShockWave(Material shockWave)
    {
        StartCoroutine(ShockWaveAction(-0.1f, 1f, shockWave));
    }

    private IEnumerator ShockWaveAction(float startPos, float endPos, Material shockWave)
    {
        shockWave.SetFloat(_waveDistanceFromCenter, startPos);
        float time = 0;
        while (time < _shockWaveTime)
        {
            var lerpedAmount = Mathf.Lerp(startPos, endPos, time / _shockWaveTime);
            shockWave.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            time += Time.deltaTime;
            yield return null;
        }
        while (time > 0)
        {
            var lerpedAmount = Mathf.Lerp(startPos, endPos, time / _shockWaveTime);
            shockWave.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            time -= Time.deltaTime;
            yield return null;
        }
    }
}