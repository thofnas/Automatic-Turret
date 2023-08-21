using System;
using CustomEventArgs;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.VFX;

namespace Turret
{
    public class TurretVisual : MonoBehaviour
    {
        [SerializeField] private VisualEffect _scannerVisualEffect;
        private static readonly int Radius = Shader.PropertyToID("Radius");

        private void Start()
        {
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Turret_OnStatUpgrade);
            GameEvents.OnTurretStatsReset.AddListener(GameEvents_Turret_OnStatReset);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            RefreshScanningRadius();
        }

        private void OnDestroy()
        {
            GameEvents.OnTurretStatUpgraded.RemoveListener(GameEvents_Turret_OnStatUpgrade);
            GameEvents.OnTurretStatsReset.RemoveListener(GameEvents_Turret_OnStatReset);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnStarted);
        }

        private void RefreshScanningRadius() => 
            _scannerVisualEffect.SetFloat(Radius, UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange));

        private void GameEvents_Turret_OnStatUpgrade(OnStatUpgradeEventArgs obj)
        {
            if (obj.Stat is Stat.ViewRange)
                RefreshScanningRadius();
        }
        
        private void GameEvents_Turret_OnStatReset(int obj) => RefreshScanningRadius();

        private void GameEvents_Wave_OnStarted() => RefreshScanningRadius();
    }
}
