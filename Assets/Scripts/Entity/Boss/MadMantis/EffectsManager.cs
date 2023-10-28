using Spriter2UnityDX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.MadMantis
{
    public class EffectsManager : MonoBehaviour
    {
        [SerializeField] private GameObject outerCircle;
        private SpriteRenderer outerCircleSprite;

        float time = 0;
        [SerializeField] private float duration = 0.1f;
        Vector3 startScale = Vector3.one * 3;
        Vector3 targetScale = Vector3.one * 10;

        [SerializeField] float lerpTime;
        [SerializeField] Color changeColor;
        private Material mantisMaterial;


        private void Start()
        {
            startScale = outerCircle.transform.localScale;
            time = 0;
            outerCircleSprite = outerCircle.GetComponent<SpriteRenderer>();

            mantisMaterial = gameObject.GetComponent<EntityRenderer>().Material;
            Color baseEnragedColor = new Color32(255, 107, 107, 100);
            mantisMaterial.SetColor("_Color", baseEnragedColor);
        }

        public void BeginDeath()
        {
            Color color2 = new Color32(107, 0, 0, 100);
            mantisMaterial.SetColor("_Color", color2);
            outerCircle.SetActive(true);
        }

        private void Update()
        {
            if (outerCircle.activeSelf)
            {
                time += Time.deltaTime / duration;

                Vector3 newScale = Vector3.Lerp(startScale, targetScale, time);
                outerCircle.transform.localScale = newScale;
                outerCircleSprite.color = Color.Lerp(outerCircleSprite.color, changeColor, lerpTime * Time.deltaTime * 3);
                 
                if (time > duration)
                {
                    enabled = false;
                    outerCircle.SetActive(false);
                }
            }
            
        }



        //private void ScaleObject(GameObject objectToScale, float speed)
        //{
            
        //}
    }
}