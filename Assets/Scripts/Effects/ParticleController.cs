using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class ParticleController : MonoBehaviour
    {
        void FlipParticle()
        {
            Vector3 originalRotation = new Vector3(gameObject.transform.eulerAngles.x,
                                                    gameObject.transform.eulerAngles.y,
                                                    gameObject.transform.eulerAngles.z);
            Vector3 newRotation = new Vector3(1.0f, -1.0f, 1.0f);
            transform.localEulerAngles = Vector3.Scale(originalRotation, newRotation);
        }

        private void OnDisable()
        {
            TransformController.OnFlip -= FlipParticle;
        }

        private void OnEnable()
        {
            TransformController.OnFlip += FlipParticle;
        }

    }
}