using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class TransformController : MonoBehaviour
    {
        private static readonly float UpdateLookTime = 0.5f;
        private Transform target;

        private void Start()
        {
            StartCoroutine(UpdateLook());
        }

        public void Flip(){
            var currentTransform = transform;
            currentTransform.Rotate(Vector3.up, 180);
            var currentScale = currentTransform.localScale;
            currentTransform.localScale = new Vector3(currentScale.x, currentScale.y, -currentScale.z);
        }

        public void LookAt(Transform target)
        {
            this.target = target;
        }

        public void LookOneTimeAt(Transform target)
        {
            var currentTransform = transform;
            if(Vector3.Dot(target.position - currentTransform.position, currentTransform.right) < 0)
                Flip();
        }

        public void StopLooking()
        {
            LookAt(null);
        }
        
        private IEnumerator UpdateLook()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateLookTime);
                
                if(CheckFlipCondition())
                    Flip();
            }
        }

        private bool CheckFlipCondition()
        {
            var currentTransform = transform;
            return target != null && Vector3.Dot(target.position - currentTransform.position, currentTransform.right) < 0;
        }
        
        
    }
}
