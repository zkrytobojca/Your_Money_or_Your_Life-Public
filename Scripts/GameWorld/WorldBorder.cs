using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    [Header("World Border Settings")]
    [SerializeField] private float radius = 160f;
    [SerializeField] private float warningRadius = 140f;
    [SerializeField] private float strength = 50f;
    
    private Vector3 centerPosition;

    private void Start()
    {
        centerPosition = transform.position;
    }

    public float Radius { get => radius; }
    public float WarningRadius { get => warningRadius; }
    public float Strength { get => strength; }
    public Vector3 CenterPosition { get => centerPosition; }
}
