using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [Header("Sound Options")]
    [SerializeField] private string soundName = string.Empty;
    [SerializeField] private bool playOnStart = false;

    void Start()
    {
        if(playOnStart && soundName != string.Empty) AudioManager.instance.PlaySound(soundName);
    }
}
