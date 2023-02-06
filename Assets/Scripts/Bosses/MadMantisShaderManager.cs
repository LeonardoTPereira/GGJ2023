using Spriter2UnityDX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MadMantisShaderManager : MonoBehaviour
{
    private float _fillRate;
    private float _step;
    private Material _material;
    private Animator _animator;

    private void Awake()
    {
        _fillRate = 0f;
        _step = 0.05f;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _material = GetComponent<EntityRenderer>().Material;
        _material.SetFloat("_FillRate", 0);
    }

    public void Enrage()
    {
        _material.SetFloat("_IsEnraging", 1);
        StartCoroutine(FillRage());
    }

    private IEnumerator FillRage()
    {
        while (_fillRate < 1.0f)
        {
            _fillRate += _step;
            _material.SetFloat("_FillRate", _fillRate);
            yield return new WaitForSeconds(2 * _step);
        }
        _animator.SetTrigger("FinishedRage");
        _material.SetFloat("_IsEnraging", 0);
    }
}