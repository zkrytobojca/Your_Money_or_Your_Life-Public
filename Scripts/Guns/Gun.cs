using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(GunModel))]
[RequireComponent(typeof(GunInputs))]
public class Gun : MonoBehaviour, IGun
{
    [Header("Gun Characteristics")]
    [SerializeField] private int damage = 5;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] private float reloadCooldown = 0.7f;
    [SerializeField] private float impactForce = 60f;
    [SerializeField] private int currentAmmo = 6;
    [SerializeField] private int maxAmmo = 6;

    [Header("Outside Objects")]
    [SerializeField] private GunModel gunModelTP;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private GameObject impactParticle;

    [Header("Photon")]
    [SerializeField] private PhotonView photonView;

    public static event Action<Vector3, Vector3> shoot;
    public static event Action hitTarget;
    public static event Action<Vector3> reload;
    public static event Action<int, int> updateAmmoValues;

    private GunModel _gunModelFP;
    private float _nextTimeToFire = 0f;
    private bool _isReloading = false;

    private GunInputs _input;

    private void Awake()
    {
        _gunModelFP = GetComponent<GunModel>();
        _input = GetComponent<GunInputs>();
    }

    private void OnEnable()
    {
        _isReloading = false;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (_nextTimeToFire >= 0) _nextTimeToFire -= Time.deltaTime;
        else if(_nextTimeToFire < 0) _nextTimeToFire = 0;

        if (_isReloading) return;

        if (_input.shoot && _nextTimeToFire <= 0)
        {
            TryShoot();
        }

        if (_input.reload)
        {
            Reload();
        }

        if (_input.scope)
        {
            Scope();
        }
    }

    private void TryShoot()
    {
        if(currentAmmo > 0)
        {
            Shoot();
        } 
        else
        {
            if(!_isReloading)
            {
                Reload();
            }
        }
    }

    public void Shoot()
    {
        _nextTimeToFire += fireCooldown;
        currentAmmo--;

        _gunModelFP.PlayMuzzleFlash();

        updateAmmoValues?.Invoke(currentAmmo, maxAmmo);

        int layerMask = (1 << 9) | (1 << 2); 
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage, hit);
                hitTarget?.Invoke();
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

            _gunModelFP.SummonBullet(_gunModelFP.GetNozzlePosition(), hit.point);
            shoot?.Invoke(gunModelTP.GetNozzlePosition(), hit.point);
        }
        else
        {
            _gunModelFP.SummonBullet(_gunModelFP.GetNozzlePosition(), fpsCam.transform.position + fpsCam.transform.forward * range);
            shoot?.Invoke(gunModelTP.GetNozzlePosition(), fpsCam.transform.position + fpsCam.transform.forward * range);
        }

        if (!_isReloading && currentAmmo == 0)
        {
            Reload();
        }
        else
        {
            _gunModelFP.Shoot();
            gunModelTP.Shoot();
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadCorutine());
        reload?.Invoke(gunModelTP.GetNozzlePosition());
    }

    IEnumerator ReloadCorutine()
    {
        _isReloading = true;
        _nextTimeToFire += reloadCooldown;
        _gunModelFP.Reload();
        gunModelTP.Reload();

        yield return new WaitForSeconds(reloadCooldown);

        currentAmmo = maxAmmo;
        updateAmmoValues?.Invoke(currentAmmo, maxAmmo);
        _isReloading = false;
    }

    public void Scope()
    {
        // TODO
    }
}
