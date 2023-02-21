using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class IntroSound : MonoBehaviour
    {
        [SerializeField] private AudioSource musicLoopSource;
        [SerializeField] private AudioClip musicIntro;

        void Start()
        {
            musicLoopSource.PlayOneShot(musicIntro);
            musicLoopSource.PlayScheduled(AudioSettings.dspTime + musicIntro.length);
        }

        public void StopTheMusic()
        {
            AudioListener.pause = true;
        }

        public void UnstopTheMusic()
        {
            //musicLoopSource.UnPause();
            AudioListener.pause = false;
        }
    }
}