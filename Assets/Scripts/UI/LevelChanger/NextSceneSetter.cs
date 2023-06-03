using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.LevelChanger
{
    public class NextSceneSetter : MonoBehaviour
    {
        public static NextSceneSetter Instance;
        [SerializeField] private bool _setNextSceneIndexManuallyBellow;
        [SerializeField] private int _nextSceneIndex;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }

        public void SetNextScene(float timeToStart = 0)
        {
            StartCoroutine(StartNextScene(timeToStart));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SetNextScene();
            }
        }

        private IEnumerator StartNextScene(float timeToStart)
        {
            yield return new WaitForSeconds(timeToStart);
  
            if (_setNextSceneIndexManuallyBellow)
                LevelChanger.Instance.FadeToLevel(_nextSceneIndex);
            else
                LevelChanger.Instance.FadeToNextLevel();
        }
    }
}