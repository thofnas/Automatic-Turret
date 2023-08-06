using System;
using System.Collections.Generic;
using System.Linq;
using CustomEventArgs;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turret
{
    [Serializable]
    public class Upgrade
    {
        public float NewStatValue;
        public int Price;
    }

    [CreateAssetMenu(menuName = "Upgrades")]
    public class Upgrades : ScriptableObject
    {
        [SerializeField] private Stat _selectedStat;
        [SerializeField] private List<Upgrade> _upgradesList = new();
        
        private readonly List<Upgrade> _appliedUpgradesList = new();

        public void UpgradeStat()
        {
            if (!TryGetNextUpgrade(out Upgrade upgrade))
                return;
            
            if(!IsEnoughMoneyForUpgrade())
                return;
            
            _appliedUpgradesList.Add(upgrade);
            GameEvents.OnStatUpgraded.Invoke(new OnStatUpgradeEventArgs { 
                Stat = _selectedStat,
                NewStatValue = upgrade.NewStatValue,
                Price = upgrade.Price
            });
        }
        
        public bool IsEnoughMoneyForUpgrade()
        {
            TryGetNextUpgrade(out Upgrade upgrade);

            if (GameManager.Instance.TotalGearAmount - upgrade.Price < 0)
            {
                Debug.Log($"Not enough money for {_selectedStat}");
                return false;
            }
            
            return true;
        }

        public bool TryGetNextUpgrade(out Upgrade upgrade)
        {
            upgrade = null;
            
            foreach (Upgrade u in _upgradesList.Where(upgrade => !_appliedUpgradesList.Contains(upgrade)))
            {
                upgrade = u;
                return true;
            }

            Debug.Log($"{_selectedStat} is already fully upgraded");
            return false;
        }

        public Upgrade GetCurrentUpgrade() => _appliedUpgradesList.LastOrDefault();

        public int GetCurrentUpgradeLevel() => _appliedUpgradesList.IndexOf(GetCurrentUpgrade()) + 1;

        public void ResetAppliedUpgrades() => _appliedUpgradesList.Clear();
    }
    
    public enum Stat
    {
        RotationSpeed,
        AmountOfHealth,
        ViewRange,
        ReloadTime,
        BulletDamage
    }
}