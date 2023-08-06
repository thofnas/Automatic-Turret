using System;
using Events;
using Turret;

namespace Managers
{
    public class UpgradeManager : Singleton<UpgradeManager>
    {
        [Serializable] public class AllTurretUpgrades : SerializableDictionary<Stat, Upgrades> {}
        public AllTurretUpgrades AllTurretUpgradesDictionary = new();

        public void Initialize()
        {
            UIEvents.OnUpgradeButtonClicked.AddListener(UIEvents_Upgrade_OnButtonClicked);
        }

        public void OnDestroy()
        {
            UIEvents.OnUpgradeButtonClicked.RemoveListener(UIEvents_Upgrade_OnButtonClicked);
        }

        private void OnApplicationQuit()
        {
            foreach (var allTurretUpgrades in AllTurretUpgradesDictionary)
            {
                allTurretUpgrades.Value.ResetAppliedUpgrades();
            }
        }

        public Upgrades GetUpgrades(Stat stat)
        {
            if (!AllTurretUpgradesDictionary.TryGetValue(stat, out Upgrades turretUpgrades)) throw new NullReferenceException();
            return turretUpgrades;
        }
        
        private void UIEvents_Upgrade_OnButtonClicked(Stat stat)
        {
            GetUpgrades(stat).UpgradeStat();
        }
    }
}