using UnityEngine;
using Assets.Scripts.Audio;
using UnityEngine.Audio;

// How to use:
// Inside some script of an object you desire to play a sound listed on the AudioManager
// you can simply type: 
// ***************** 'AudioManager.Instance.Play("Name of the sound");' ******************
// passing the name of the sound to play it.

public class AudioManager : Audios
{
    // 'instance' references to itself
    public static AudioManager Instance;

    [SerializeField] private AudioMixer _mixer;

    // Awake is called before the Start method
    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            GameObject.Destroy(this);

        base.Awake();
    }

    public AudioMixer GetMainMixer()
    {
        return _mixer;
    }
}