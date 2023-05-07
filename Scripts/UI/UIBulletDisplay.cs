using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIBulletDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Gun.updateAmmoValues += UpdateAmmoDisplay;
    }

    private void OnDisable()
    {
        Gun.updateAmmoValues -= UpdateAmmoDisplay;
    }

    private void UpdateAmmoDisplay(int current, int max)
    {
        text.text = current.ToString() + " / " + max.ToString();
    }
}
