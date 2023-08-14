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
            _scannerVisualEffect.SetFloat(Radius, UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange));
        }

        private void OnDestroy()
        {
            GameEvents.OnTurretStatUpgraded.RemoveListener(GameEvents_Turret_OnStatUpgrade);
        }
        
        private void GameEvents_Turret_OnStatUpgrade(OnStatUpgradeEventArgs obj)
        {
            if (obj.Stat.Equals(Stat.ViewRange))
                _scannerVisualEffect.SetFloat(Radius, UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange));
        }
    }
}
