using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        if (Camera.main == null) return;
        mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (mainCameraTransform == null)
        {
            if (Camera.main == null) return;
            mainCameraTransform = Camera.main.transform;
        }
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}
