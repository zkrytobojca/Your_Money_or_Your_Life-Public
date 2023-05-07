using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorderWarning : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject worldBorderWarning;

    private void OnEnable()
    {
        WorldBorderEffect.updateWarningText += ShowWarning;
    }

    private void OnDisable()
    {
        WorldBorderEffect.updateWarningText -= ShowWarning;
    }

    private void ShowWarning(bool doShow)
    {
        worldBorderWarning.SetActive(doShow);
    }
}
