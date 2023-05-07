using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AimTargetMovement : MonoBehaviour
{
    [Header("Outside Objects")]
    public Camera fpsCam;
    [Header("Photon")]
    public PhotonView photonView;

    [Header("Aim Restraints")]
    [SerializeField] private float range = 100f;
    [SerializeField] private float minRange = 2f;

    void Update()
    {
        if (photonView.IsMine)
        {
            transform.rotation = fpsCam.transform.rotation;

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                transform.position = hit.distance > minRange ? hit.point : fpsCam.transform.position + fpsCam.transform.forward * minRange;
            }
            else
            {
                transform.position = fpsCam.transform.position + fpsCam.transform.forward * range;
            }
        }
    }
}
