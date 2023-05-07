using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Header("Camera Settings")]
    [SerializeField] private float movementSpeed = 1f;

    private CinemachineTrackedDolly dollyCamera;
    private bool isDuringAnimation = false;
    private int currentState = 0;
    private float lerpState = 0f;

    private void Start()
    {
        dollyCamera = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    public void ChangeState(int newState)
    {
        currentState = newState;
        isDuringAnimation = true;
    }

    private void MoveCamera()
    {
        if (!isDuringAnimation) return;

        for(int i = 0; i != targetGroup.m_Targets.Length; i++)
        {
            if(i == currentState)
            {
                targetGroup.m_Targets[i].weight = Mathf.Clamp01(targetGroup.m_Targets[i].weight + Time.deltaTime * movementSpeed);
            }
            else
            {
                targetGroup.m_Targets[i].weight = Mathf.Clamp01(targetGroup.m_Targets[i].weight - Time.deltaTime * movementSpeed);
            }
        }

        lerpState += Mathf.Clamp01(Time.deltaTime * movementSpeed);
        dollyCamera.m_PathPosition = Mathf.Lerp(dollyCamera.m_PathPosition, currentState, lerpState);

        if (targetGroup.m_Targets[currentState].weight == 1 && dollyCamera.m_PathPosition == currentState) 
        {
            isDuringAnimation = false;
            lerpState = 0f;
        }
    }
}
