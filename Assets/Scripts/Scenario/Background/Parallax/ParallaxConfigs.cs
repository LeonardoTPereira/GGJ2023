using UnityEngine;

namespace Assets.Scripts.Scenario
{
    public class ParallaxConfigs : MonoBehaviour
    {
        public static ParallaxConfigs Instance;

        // Background "chunk" sprite size in X axis
        [HideInInspector] public float chunkSize;
        [HideInInspector] public GameObject mainCamera;

        [Header("Parallax Factor in X axis")]
        [Range(0.0f, 1.0f)]
        public float closestX;
        [Range(0.0f, 1.0f)]
        public float closeX;
        [Range(0.0f, 1.0f)]
        public float middleCloseX;
        [Range(0.0f, 1.0f)]
        public float middleX;
        [Range(0.0f, 1.0f)]
        public float middleFarX;
        [Range(0.0f, 1.0f)]
        public float farX;
        [Range(0.0f, 1.0f)]
        public float farthestX;

        // Value multiplied with the X factor to get the Y factor
        [Header("Parallax Factor in Y axis")]
        [Range(0.0f, 1.0f)]
        public float yFactor = 0.75f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void OnEnable()
        {
            chunkSize = gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            gameObject.transform.SetParent(mainCamera.transform);
        }
    }
}