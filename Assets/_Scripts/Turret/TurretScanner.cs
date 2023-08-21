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
            GameEvents.OnTurretStatsReset.AddListener(GameEvents_Turret_OnStatReset);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            _scannerCollider = gameObject.GetComponent<SphereCollider>();
            RefreshScanningRadius();
        }
        
        private void OnDestroy()
        {
            GameEvents.OnTurretStatUpgraded.RemoveListener(GameEvents_Turret_OnStatUpgrade);
            GameEvents.OnTurretStatsReset.RemoveListener(GameEvents_Turret_OnStatReset);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnStarted);
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

        private void RefreshScanningRadius() => _scannerCollider.radius = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange) / 20f;

        private void GameEvents_Turret_OnStatUpgrade(OnStatUpgradeEventArgs obj)
        {
            if (obj.Stat.Equals(Stat.ViewRange))
                RefreshScanningRadius();
        }
        
        private void GameEvents_Turret_OnStatReset(int obj) => RefreshScanningRadius();
        
        private void GameEvents_Wave_OnStarted() => RefreshScanningRadius();
    }
}

