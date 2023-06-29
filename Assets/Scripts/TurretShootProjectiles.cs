using System;
using CustomEventArgs;
using UnityEngine;

public class TurretShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    private Turret _turret;

    private void Awake()
    {
        _turret = GetComponent<Turret>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        _turret.OnShoot += TurretShootProjectiles_OnShoot;
    }

    private void OnDisable()
    {
        _turret.OnShoot -= TurretShootProjectiles_OnShoot;
    }

    private void TurretShootProjectiles_OnShoot(object sender, OnShootEventArgs e)
    {
        var bulletGameObject = Instantiate(_bulletPrefab, e.GunEndPointPosition, Quaternion.identity);

        var shootDir = (e.GunEndPointPosition - e.GunStartPointPosition).normalized;
        bulletGameObject.GetComponent<Bullet>().Setup(shootDir);
    }
}
