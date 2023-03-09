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
            GameObject.Destroy(gameObject);

        base.Awake();
    }

    // Converts UI Toolkit slider value to Volume(audioMixerGroup) value
    public float ConvertSliderValueToVolume(float sliderValue)
    {
        return Mathf.Log10((sliderValue + 0.01f) / 100) * 20;
    }

    // Converts Volume(audioMixerGroup) value to UI Toolkit slider value
    public float ConvertVolumeToSliderValue(float volumeValue)
    {
        return Mathf.Pow(10, volumeValue / 20 + 2);
    }

    public void SetVolume(string audioMixerParameterName, float volume, bool isSlider )
    {
        if (isSlider)
            volume = ConvertSliderValueToVolume(volume);

        _mixer.SetFloat(audioMixerParameterName, volume);
    }

    public AudioMixer GetMainMixer()
    {
        return _mixer;
    }
}