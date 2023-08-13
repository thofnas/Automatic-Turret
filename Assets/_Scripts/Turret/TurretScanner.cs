using Events;
using Managers;
using UnityEngine;

namespace Turret
{
    public class TurretScanner : MonoBehaviour
    {
        private SphereCollider _scannerCollider;

        private void Start()
        {
            _scannerCollider = gameObject.GetComponent<SphereCollider>();
            _scannerCollider.radius = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange) / 20f;
        }

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

