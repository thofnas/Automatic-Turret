using Events;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                GameEvents.OnEnemySpotted.Invoke(enemy);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                GameEvents.OnEnemyLostFromView.Invoke(enemy);
            }
        }
    }
}

