using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulser : MonoBehaviour
{
    [SerializeField] private float duration;
    private Light2D light;
    private float offset = 2f;

    void Start()
    {
        light = GetComponent<Light2D>();
    }

    void FixedUpdate()
    {
        float phi = (Time.time / duration) * Mathf.PI;
        float amplitude = (Mathf.Cos(phi) * 0.5f + offset)/2;
        light.intensity = amplitude;
    }
}
