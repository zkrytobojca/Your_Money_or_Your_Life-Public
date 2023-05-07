using Photon.Pun;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacterInputs))]
public class Stamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float regenerationRate = 1f;

    [Header("Photon")]
    public PhotonView photonView;

    private float currentStamina = 0f;

    private PlayerCharacterInputs _input;

    public static event Action<float> staminaChange;

    private void Start()
    {
        currentStamina = maxStamina;
        if (photonView.IsMine)
        {
            _input = GetComponent<PlayerCharacterInputs>();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (_input.sprint)
            {
                currentStamina -= Time.deltaTime;
                if (currentStamina < 0) currentStamina = 0;
                staminaChange?.Invoke(currentStamina / maxStamina);
            }
            else if (currentStamina < maxStamina)
            {
                currentStamina += regenerationRate * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
                staminaChange?.Invoke(currentStamina / maxStamina);
            }
        }
    }

    public float getCurrentStamina()
    {
        return currentStamina;
    }
}
