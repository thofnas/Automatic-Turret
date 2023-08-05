using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;

namespace Turret
{

    
    [CreateAssetMenu(menuName = "Upgrade")]
    public class Upgrade : ScriptableObject
    {
        [Serializable] public class TurretStats : SerializableDictionary<Stat, float> {}
        public TurretStats TurretStatsDictionary;
    }

    [CreateAssetMenu(menuName = "Upgrades")]
    public class Upgrades : ScriptableObject
    {
        public List<Upgrade> UpgradesList = new();
        private List<Upgrade> _appliedUpgrades = new();

        public void ApplyUpgrade(Stat stat)
        {
            foreach (Upgrade upgrade in UpgradesList.Where(upgrade => !_appliedUpgrades.Contains(upgrade)))
            {
                _appliedUpgrades.Add(upgrade);
                GameEvents.OnStatUpgraded.Invoke(stat);
                break;
            }
            Debug.Log($"{stat.ToString()} is already fully upgraded");
        }

        public Upgrade GetAppliedUpgrade(Stat stat)
        {
            Upgrade lastUpgrade = null;
            
            foreach (Upgrade upgrade in _appliedUpgrades.Where(upgrade => upgrade.TurretStatsDictionary.ContainsKey(stat)))
            {
                lastUpgrade = upgrade;
            }
            
            if (lastUpgrade == null) throw new Exception("");
            
            return lastUpgrade;
        }

        public void ResetAppliedUpgrades() => _appliedUpgrades.Clear();
    }

    public class UpgradeManager : Singleton<UpgradeManager>
    {
        [Serializable] public class AllTurretUpgrades : SerializableDictionary<Stat, Upgrades> {}
        public AllTurretUpgrades AllTurretUpgradesDictionary = new();

        private void OnApplicationQuit()
        {
            foreach (var allTurretUpgrades in AllTurretUpgradesDictionary)
            {
                allTurretUpgrades.Value.ResetAppliedUpgrades();
            }
        }

        public Upgrades GetUpgrades(Stat stat)
        {
            foreach (var allTurretUpgrades in AllTurretUpgradesDictionary)
            {
                if (allTurretUpgrades.Key == stat) return allTurretUpgrades.Value;
            }

            throw new Exception("It shouldn't have happened.");
        }
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