using Events;
using UnityEngine;

namespace Turret
{
    public class TurretShootProjectiles : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;

        private void OnEnable() => GameEvents.TurretOnShoot.AddListener(GameEvents_TurretOnShoot);

        private void OnDisable() => GameEvents.TurretOnShoot.RemoveListener(GameEvents_TurretOnShoot);

        private void GameEvents_TurretOnShoot(Transform gunEndPoint)
        {
            GameObject bulletGameObject = Instantiate(_bulletPrefab, gunEndPoint.position, Quaternion.identity);

            Vector3 shootDir = gunEndPoint.transform.forward;
            
            bulletGameObject.GetComponent<Bullet>().Setup(shootDir);
        }
    }
}
