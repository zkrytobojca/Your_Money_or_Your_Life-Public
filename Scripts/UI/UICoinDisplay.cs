using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UICoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        PlayerCoins.updateCoinsUI += UpdateCoins;
    }

    private void OnDisable()
    {
        PlayerCoins.updateCoinsUI -= UpdateCoins;
    }

    private void UpdateCoins(int amount)
    {
        text.text = amount.ToString();
    }
}
