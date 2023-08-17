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
            GameEvents.OnTurretShoot.AddListener(GameEvents_TurretOnShoot);
            InitPool(_bulletPrefab, 5, 50, true);
        }

        private void OnDestroy() => GameEvents.OnTurretShoot.RemoveListener(GameEvents_TurretOnShoot);
        
        protected override Bullet CreateSetup() => Instantiate(_bulletPrefab, _gunEndPoint.position, Quaternion.identity);
        
        protected override void GetSetup(Bullet bullet)
        {
            base.GetSetup(bullet);
            bullet.ResetBullet();
            
            bullet.transform.position = _gunEndPoint.position;
            bullet.SetPool(Pool);

            bullet.Setup(_gunEndPoint.transform.forward);
        }

        protected override void ReleaseSetup(Bullet bullet)
        {
            base.ReleaseSetup(bullet);
            bullet.ResetBullet();
        }

        private void GameEvents_TurretOnShoot(Transform gunEndPoint)
        {
            _gunEndPoint = gunEndPoint;
            print(Pool.GetType());

            Get();
            print(Pool.GetType());
        }
    }
}
