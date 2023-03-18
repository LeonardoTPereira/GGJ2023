using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private float damageTime;  
    private EntityRenderer entityRenderer;

    private void Start()
    {
        entityRenderer = GetComponent<EntityRenderer>();
    }

    public void BlinkDamage()
    {
        Debug.Log("Blink Player");
        StartCoroutine(ChangeColor(damageTime));
    }

   IEnumerator ChangeColor(float damageTime)
    {
        entityRenderer.Material.SetFloat("_Hit", 1);
        yield return new WaitForSeconds(damageTime);
        entityRenderer.Material.SetFloat("_Hit", 0);
    }
}
