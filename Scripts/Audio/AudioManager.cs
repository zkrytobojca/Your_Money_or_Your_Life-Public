using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<Sound> sounds;

    [HideInInspector]
    public static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
        
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.looping;
            sound.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[(int)sound.type+1];
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume")) audioMixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("volume")) * 20);
        if (PlayerPrefs.HasKey("volume_game")) audioMixer.SetFloat("VolumeGame", Mathf.Log10(PlayerPrefs.GetFloat("volume_game")) * 20);
        if (PlayerPrefs.HasKey("volume_ui")) audioMixer.SetFloat("VolumeUI", Mathf.Log10(PlayerPrefs.GetFloat("volume_ui")) * 20);
        if (PlayerPrefs.HasKey("volume_music")) audioMixer.SetFloat("VolumeMusic", Mathf.Log10(PlayerPrefs.GetFloat("volume_music")) * 20);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (instance == null) return;
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        if (sound.source.loop == true && sound.source.isPlaying) return;
        sound.source.Play();
    }

    public void PlaySound(string name, Vector3 position)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        GameObject soundObject = new GameObject("Sound");
        soundObject.transform.position = position;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = sound.audioClip;
        audioSource.pitch = sound.pitch;
        audioSource.volume = sound.volume;
        audioSource.loop = sound.looping;
        audioSource.maxDistance = 1200f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.dopplerLevel = 0f;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[(int)sound.type + 1];
        audioSource.Play();

        Destroy(soundObject, audioSource.clip.length);
    }

    public void PlayRandomSound(string name)
    {
        List<Sound> soundList = sounds.FindAll(s => s.name.Contains(name));
        if (soundList == null)
        {
            Debug.LogWarning("Sound containing name " + name + " not found!");
            return;
        }
        soundList[Random.Range(0, soundList.Count)].source.Play();
    }

    public void PlayRandomSound(string name, Vector3 position)
    {
        List<Sound> soundList = sounds.FindAll(s => s.name.Contains(name));
        if (soundList == null)
        {
            Debug.LogWarning("Sound containing name " + name + " not found!");
            return;
        }
        Sound sound = soundList[Random.Range(0, soundList.Count)];
        
        GameObject soundObject = new GameObject("Sound");
        soundObject.transform.position = position;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = sound.audioClip;
        audioSource.pitch = sound.pitch;
        audioSource.volume = sound.volume;
        audioSource.loop = sound.looping;
        audioSource.maxDistance = 1200f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.dopplerLevel = 0f;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[(int)sound.type + 1];
        audioSource.Play();

        Destroy(soundObject, audioSource.clip.length);
    }

    public void StopSound(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        sound.source.Stop();
    }

    public void PauseSound(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        sound.source.Pause();
    }

    public void UnPauseSound(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }
        sound.source.UnPause();
    }
}
