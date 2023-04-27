using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.LevelChanger
{
    public class NextSceneSetter : MonoBehaviour
    {
        [SerializeField] private bool _setNextSceneIndexManuallyBellow;
        [SerializeField] private int _nextSceneIndex;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_setNextSceneIndexManuallyBellow)
                    LevelChanger.Instance.FadeToLevel(_nextSceneIndex);
                else
                    LevelChanger.Instance.FadeToNextLevel();
            }
        }
    }
}