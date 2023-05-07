using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject crosshairHit;
    [SerializeField] private float timeOfHitIndicator = 0.25f;

    private void OnEnable()
    {
        Gun.hitTarget += OnHitTarget;
    }

    private void OnDisable()
    {
        Gun.hitTarget -= OnHitTarget;
    }

    public void OnHitTarget()
    {
        StartCoroutine(BlinkCrosshairHit(timeOfHitIndicator));
    }

    IEnumerator BlinkCrosshairHit(float time)
    {
        crosshairHit.SetActive(true);
        yield return new WaitForSeconds(time);
        crosshairHit.SetActive(false);
    }
}
