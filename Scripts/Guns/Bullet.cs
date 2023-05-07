using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Characteristics")]
    [SerializeField] private float speed = 1000f;

    private Vector3 target;
    private Vector3 start;
    private bool isStartSet;
    private TrailRenderer trailRenderer;

    private void Start()
    {
        if (!isStartSet)
        {
            start = transform.position;
            isStartSet = true;
        }
    }

    private void Update()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        // calculate distance to move
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // fix for lag compensation issue - temporary
        if (trailRenderer != null)
        {
            trailRenderer.SetPosition(0, start);
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void SetStart(Vector3 start)
    {
        this.start = start;
        isStartSet = true;
    }
}
