using Turret;

namespace Events
{
    public abstract class UIEvents
    {
        public static readonly EventRecorder OnStartWaveButtonClicked = new();
        public static readonly EventRecorder OnResetUpgradesButtonClicked = new();
        public static readonly EventRecorder<Stat> OnUpgradeButtonClicked = new();
    }
}
