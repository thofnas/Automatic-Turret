using System;
using CustomEventArgs;
using UnityEngine;

public class TurretShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    private Turret _turret;

    private void Awake() => _turret = GetComponent<Turret>();

    private void OnEnable() => _turret.OnShoot += TurretShootProjectiles_OnShoot;

    private void OnDisable() => _turret.OnShoot -= TurretShootProjectiles_OnShoot;

    private void TurretShootProjectiles_OnShoot(object sender, OnShootEventArgs e)
    {
        GameObject bulletGameObject = Instantiate(_bulletPrefab, e.GunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (e.GunEndPointPosition - e.GunStartPointPosition).normalized;
        bulletGameObject.GetComponent<Bullet>().Setup(shootDir);
    }
}
