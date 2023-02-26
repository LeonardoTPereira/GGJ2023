using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenario
{
    public class ParallaxConfigs: MonoBehaviour
    {
        public static ParallaxConfigs Instance;
        [Header("Parallax Factor in X axis")]
        [Range(0.0f, 1.0f)]
        public float closestX;
        [Range(0.0f, 1.0f)]
        public float closeX;
        [Range(0.0f, 1.0f)]
        public float farthestX;


        [Header("Parallax Factor in X axis")]
        [Range(0.0f, 1.0f)]
        public float closestY;
        [Range(0.0f, 1.0f)]
        public float closeY;
        [Range(0.0f, 1.0f)]
        public float farthestY;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }
    }
}
