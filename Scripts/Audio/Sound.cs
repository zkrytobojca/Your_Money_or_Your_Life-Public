using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum SoundType
    {
        Game,
        UI,
        Music
    }

    public string name;

    public AudioClip audioClip;

    public SoundType type;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(0.1f, 3f)]
    public float pitch = 1;

    public bool looping = false;

    [HideInInspector]
    public AudioSource source;
}
