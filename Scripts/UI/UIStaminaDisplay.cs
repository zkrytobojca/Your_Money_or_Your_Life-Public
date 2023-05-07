using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStaminaDisplay : MonoBehaviour
{
    [Header("Stamina Bar")]
    [SerializeField] private Transform staminaBar;
    [SerializeField] private Transform staminaBarBG;

    private float currentPercent = 1f;

    private void OnEnable()
    {
        Stamina.staminaChange += UpdateStaminaBar;
    }

    private void OnDisable()
    {
        Stamina.staminaChange -= UpdateStaminaBar;
    }

    private void UpdateStaminaBar(float percent)
    {
        currentPercent = percent;
        staminaBar.localScale = new Vector3(percent, 1f, 1f);

        if (currentPercent == 1)
        {
            staminaBar.gameObject.SetActive(false);
            staminaBarBG.gameObject.SetActive(false);
        }
        else if (!staminaBar.gameObject.activeInHierarchy)
        {
            staminaBar.gameObject.SetActive(true);
            staminaBarBG.gameObject.SetActive(true);
        }
    }
}
