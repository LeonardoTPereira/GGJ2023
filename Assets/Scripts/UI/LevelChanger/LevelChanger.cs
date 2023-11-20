using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.LevelChanger
{
    public class LevelChanger : MonoBehaviour
    {
        public static LevelChanger Instance;

        private const int DEFAULT_LEVEL_INDEX = -1;

        [SerializeField] private Animator _animator;
        private int _nextLevelIndex = DEFAULT_LEVEL_INDEX;
        [SerializeField] private GameObject ObjectToDestroy;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        public void FadeToLevel(int nextLevelIndex)
        {
            _nextLevelIndex = nextLevelIndex;
            _animator.SetTrigger("FadeOut");
        }

        public void FadeToNextLevel()
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
            Destroy(ObjectToDestroy);
        }

        public void OnFadeComplete()
        {
            if (_nextLevelIndex == DEFAULT_LEVEL_INDEX)
            {
                Debug.Log("NEXT LEVEL NOT SETTED ON LevelChanger");
                return;
            }
            SceneManager.LoadScene(_nextLevelIndex);
        }
    }
}