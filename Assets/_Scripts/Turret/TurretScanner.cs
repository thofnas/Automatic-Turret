using _Events;
using _Managers;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                //EnemyManager.Instance.AddEnemy(enemy);
                GameEvents.OnEnemySpotted.Invoke();
            }
        }
    }
}
