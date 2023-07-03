using System;
using CustomEventArgs;
using UnityEngine;

public class TurretShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    private void OnEnable() => Turret.OnShoot += TurretShootProjectiles_OnShoot;

    private void OnDisable() => Turret.OnShoot -= TurretShootProjectiles_OnShoot;

    private void TurretShootProjectiles_OnShoot(object sender, OnShootEventArgs e)
    {
        GameObject bulletGameObject = Instantiate(_bulletPrefab, e.GunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (e.GunEndPointPosition - e.GunStartPointPosition).normalized;
        bulletGameObject.GetComponent<Bullet>().Setup(shootDir);
    }
}
