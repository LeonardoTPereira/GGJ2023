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

    [SerializeField] private AudioMixerGroup _Main_output;

    // Awake is called before the Start method
    protected override void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            GameObject.Destroy(this);

        base.Awake();
    }

    public AudioMixerGroup GetMainMixer()
    {
        return _Main_output;
    }

    public AudioMixerGroup GetMusicMixer()
    {
        return _Music_output;
    }

    public AudioMixerGroup GetSfxMixer() 
    {
        return _SFX_output;
    }
}