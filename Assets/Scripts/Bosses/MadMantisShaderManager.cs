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
        _material = GetComponent<Renderer>().material;
        _animator = GetComponent<Animator>();
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
            _material.SetFloat("_FillRate", _fillRate);
            _fillRate += _step;
            yield return new WaitForSeconds(2 * _step);
        }
        _animator.SetTrigger("FinishedRage");
        _material.SetFloat("_IsEnraging", 0);
    }
}