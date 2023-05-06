using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulser : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float minValue, maxValue, offset;
    private Light2D light;

    void Start()
    {
        light = GetComponent<Light2D>();
    }

    void FixedUpdate()
    {
        float phi = ((Time.time / duration) * Mathf.PI) + offset;
        //float amplitude = (Mathf.Cos(phi) * 0.5f + offset)/2;
        float amplitude = (((Mathf.Cos(phi) + 1 ) * (maxValue - minValue))/ 2) + minValue;
        light.intensity = amplitude;
    }
}
