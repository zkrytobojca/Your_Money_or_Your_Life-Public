using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GunModel : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Transform nozzle;
    [SerializeField] private ParticleSystem muzzleFlash;
    [Header("Bullets")]
    [SerializeField] private GameObject bulletPrefab;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public Vector3 GetNozzlePosition()
    {
        return nozzle.position;
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
    }

    public void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    public void SummonBullet(Vector3 start, Vector3 target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, start, Quaternion.identity);
        bulletGO.transform.LookAt(target);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetStart(start);
            bullet.SetTarget(target);
        }
        Destroy(bulletGO, 2f);
    }
}
