using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.MadMantis
{
    public class EffectsManager : MonoBehaviour
    {
        //[SerializeField] private GameObject innerCircle;
        [SerializeField] private GameObject outerCircle;
        private Color outerCircleSprite;
        float alfa = 255;

        float time = 0;
        [SerializeField] private float duration = 3f;
        Vector3 startScale, targetScale = Vector3.one * 5;

        private void Start()
        {
            startScale = outerCircle.transform.localScale;
            time = 0;
            outerCircleSprite = outerCircle.GetComponent<SpriteRenderer>().color;
        }
        public void BeginDeath()
        {
            
            //innerCircle.SetActive(true);
            outerCircle.SetActive(true);
            //fazer o circulo aumentar de tamanho até um valor específico, crescimento depende da velocidade
            // fazer o circulo desaparecer conforme o tempo
        }

        private void Update()
        {
            if (outerCircle.activeSelf)
            {
                time += Time.deltaTime / duration;

                Vector3 newScale = Vector3.Lerp(startScale, targetScale, time);
                outerCircle.transform.localScale = newScale;
                outerCircleSprite.a -= 1f;
                if (time > 1)
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