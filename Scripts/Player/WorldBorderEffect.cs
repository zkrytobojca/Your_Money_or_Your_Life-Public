using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class WorldBorderEffect : MonoBehaviour
{
    [Header("FX")]
    [SerializeField] private GameObject fx;
    [SerializeField] private float fxDisplacement = 50f;
    [Range(0f, 1f)]
    [SerializeField] private float fxLerpSpeed = .1f;
    [Header("Photon")]
    [SerializeField] private PhotonView photonView;

    private WorldBorder worldBorder;
    private Vector3 currentVelocity;
    private bool isNearPlayer = false;

    public static Action<bool> updateWarningText;

    private void Start()
    {
        worldBorder = FindObjectOfType<WorldBorder>();
    }

    private void Update()
    {
        //if(!photonView.IsMine) return;

        float distance = Vector3.Distance(transform.position, worldBorder.CenterPosition);

        if (distance >= worldBorder.WarningRadius)
        {
            if (photonView.IsMine) updateWarningText?.Invoke(true);

            fx.SetActive(true);
            Vector3 fxTargetPosition = (worldBorder.CenterPosition + transform.position).normalized * (worldBorder.Radius + fxDisplacement);
            if (!isNearPlayer)
            {
                fx.transform.position = fxTargetPosition;
                currentVelocity = new Vector3();
                isNearPlayer = true;
            }
            else
            {
                fx.transform.position = Vector3.SmoothDamp(fx.transform.position, fxTargetPosition, ref currentVelocity, fxLerpSpeed);
            }
            fx.transform.LookAt(worldBorder.CenterPosition);
        }
        else
        {
            if (photonView.IsMine) updateWarningText?.Invoke(false);
            fx.SetActive(false);
            isNearPlayer = false;
        }

        if (distance >= worldBorder.Radius)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldBorder.CenterPosition, worldBorder.Strength * Time.deltaTime);
        }
    }
}
