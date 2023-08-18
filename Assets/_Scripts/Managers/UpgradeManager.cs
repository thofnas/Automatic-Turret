using System;
using Events;
using Turret;
using UnityEngine;

namespace Managers
{
    public class UpgradeManager : Singleton<UpgradeManager>
    {
        [Serializable] public class AllTurretUpgrades : SerializableDictionary<Stat, TurretUpgrades> {}
        public AllTurretUpgrades AllTurretUpgradesDictionary = new();
        [SerializeField] private TurretBaseStats _turretBaseStats;

        public void Initialize()
        {
            UIEvents.OnUpgradeButtonClicked.AddListener(UIEvents_Upgrade_OnButtonClicked);
        }

        private void OnDestroy()
        {
            UIEvents.OnUpgradeButtonClicked.RemoveListener(UIEvents_Upgrade_OnButtonClicked);
        }

        private void OnApplicationQuit()
        {
            ResetAllUpgrades(out int _);
        }

        public int GetPriceForAllUpgrades()
        {
            float totalPrice = 0;
            
            foreach (var allTurretUpgrades in AllTurretUpgradesDictionary)
            {
                foreach (Upgrade upgrade in allTurretUpgrades.Value.GetAllUpgrades())
                {
                    totalPrice += upgrade.UpgradePrice;
                }
            }

            return Mathf.FloorToInt(totalPrice);
        }

        public void ResetAllUpgrades(out int refund)
        {
            float totalRefund = 0;
            
            foreach (var allTurretUpgrades in AllTurretUpgradesDictionary)
            {
                allTurretUpgrades.Value.ResetAppliedUpgrades(out float priceForStats);
                totalRefund += priceForStats;
            }

            refund = Mathf.FloorToInt(totalRefund);
        }

        public TurretUpgrades GetUpgradesForStat(Stat stat)
        {
            if (!AllTurretUpgradesDictionary.TryGetValue(stat, out TurretUpgrades turretUpgrades)) throw new NullReferenceException();
            return turretUpgrades;
        }
        
        public float GetTurretUpgradedStat(Stat stat)
        {
            _turretBaseStats.TurretBaseStatsDictionary.TryGetValue(stat, out float baseValue);

            if (!GetUpgradesForStat(stat).TryGetCurrentUpgrade(out Upgrade upgrade))
                return baseValue;
            
            return upgrade.NumberThatUpgradesBaseValue + baseValue;
        }

        private void UIEvents_Upgrade_OnButtonClicked(Stat stat)
        {
            GetUpgradesForStat(stat).UpgradeStat();
        }
    }
}