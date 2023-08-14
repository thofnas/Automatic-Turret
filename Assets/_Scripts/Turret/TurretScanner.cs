using CustomEventArgs;
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
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Turret_OnStatUpgrade);
            _scannerCollider = gameObject.GetComponent<SphereCollider>();
            _scannerCollider.radius = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange) / 20f;
        }
        
        private void OnDestroy()
        {
            GameEvents.OnTurretStatUpgraded.RemoveListener(GameEvents_Turret_OnStatUpgrade);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy.Enemy enemy))
            {
                GameEvents.OnEnemySpotted.Invoke(enemy);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Enemy.Enemy enemy))
            {
                GameEvents.OnEnemyLostFromView.Invoke(enemy);
            }
        }

        private void GameEvents_Turret_OnStatUpgrade(OnStatUpgradeEventArgs obj)
        {
            if (obj.Stat.Equals(Stat.ViewRange))
                _scannerCollider.radius = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange);
        }
    }
}

