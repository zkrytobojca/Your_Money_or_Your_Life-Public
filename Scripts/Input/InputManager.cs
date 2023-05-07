using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public static InputManager instance;

    [Header("Actions")]
    public InputActionAsset inputActions;

    [Header("Debug")]
    [SerializeField] private bool resetPlayerPrefs = false;
    [SerializeField] private bool resetBindingOverrites = false;

    private const string RebindsKey = "rebinds";

    public static Action rebindingsReset;

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

        if (resetPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }

        LoadRebinds();

        if(resetBindingOverrites)
        {
            ResetAllBindigns();
        }
    }
    private void OnApplicationQuit()
    {
        SaveRebinds();
    }

    private void LoadRebinds()
    {
        string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);

        if (string.IsNullOrEmpty(rebinds)) return;

        inputActions.LoadBindingOverridesFromJson(rebinds);
    }

    public void SaveRebinds()
    {
        string rebinds = inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(RebindsKey, rebinds);
    }

    public void ResetAllBindigns()
    {
        inputActions.RemoveAllBindingOverrides();

        rebindingsReset?.Invoke();
    }
}
