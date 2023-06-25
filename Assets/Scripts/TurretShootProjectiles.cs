using UnityEngine;

public class TurretShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    
    private void Start()
    {
        GetComponent<Turret>().OnShoot += TurretShootProjectiles_OnShoot;
    }
    
    private void TurretShootProjectiles_OnShoot(object sender, Turret.OnShootEventArgs e)
    {
        var bulletGameObject = Instantiate(_bulletPrefab, e.GunEndPointPosition, Quaternion.identity);

        var shootDir = (e.GunEndPointPosition - e.GunStartPointPosition).normalized;
        bulletGameObject.GetComponent<Bullet>().Setup(shootDir);
    }
}
