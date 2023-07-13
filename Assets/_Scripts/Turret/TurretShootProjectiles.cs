using _CustomEventArgs;
using _Events;
using Turret.StateMachine.States;
using UnityEngine;

namespace Turret
{
    public class TurretShootProjectiles : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;

        private void OnEnable() => GameEvents.TurretOnShoot.AddListener(GameEvents_TurretOnShoot);

        private void OnDisable() => GameEvents.TurretOnShoot.RemoveListener(GameEvents_TurretOnShoot);

        private void GameEvents_TurretOnShoot(OnShootEventArgs e)
        {
            GameObject bulletGameObject = Instantiate(_bulletPrefab, e.GunEndPointPosition, Quaternion.identity);

            Vector3 shootDir = (e.GunEndPointPosition - e.GunStartPointPosition).normalized;
            bulletGameObject.GetComponent<Bullet>().Setup(shootDir);
        }
    }
}
