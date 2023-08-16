using Events;
using UnityEngine;

namespace Turret
{
    public class TurretShootProjectiles : PoolerBase<Bullet>
    {
        [SerializeField] private Bullet _bulletPrefab;

        private Transform _gunEndPoint;

        public void Initialize()
        {
            GameEvents.TurretOnShoot.AddListener(GameEvents_TurretOnShoot);
            InitPool(_bulletPrefab, 5, 50, true);
        }

        private void OnDestroy() => GameEvents.TurretOnShoot.RemoveListener(GameEvents_TurretOnShoot);
        
        protected override Bullet CreateSetup() => Instantiate(_bulletPrefab, _gunEndPoint.position, Quaternion.identity);
        
        protected override void GetSetup(Bullet bullet)
        {
            base.GetSetup(bullet);

            bullet.transform.position = _gunEndPoint.position;
            bullet.SetPool(Pool);

            bullet.Setup(_gunEndPoint.transform.forward);
        }

        private void GameEvents_TurretOnShoot(Transform gunEndPoint)
        {
            _gunEndPoint = gunEndPoint;

            Get();
        }
    }
}
