using _Events;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : Singleton<TurretScanner>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                GameEvents.OnEnemySpotted.Invoke(enemy);
            }
        }
    }
}
