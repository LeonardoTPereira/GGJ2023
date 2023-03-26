using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace UI.Menu.Play.Pause
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance;
        public bool IsPaused;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            Instance = this;

            IsPaused = false;
        }

        public void PauseOrUnpause()
        {
            Time.timeScale = 0;
            if (IsPaused)
                Time.timeScale = 1;

            IsPaused = !IsPaused;
        }

        public void PressPauseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PauseOrUnpause();
            }
        }
    }
}
