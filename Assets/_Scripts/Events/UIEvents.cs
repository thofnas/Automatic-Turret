using Turret;

namespace Events
{
    public abstract class UIEvents
    {
        public static readonly EventRecorder OnStartWaveButtonClicked = new();
        public static readonly EventRecorder OnResetUpgradesButtonClicked = new();
        public static readonly EventRecorder<Stat> OnUpgradeButtonClicked = new();

        public static readonly EventRecorder OnReturnToLobbyButtonClicked = new();
        public static readonly EventRecorder OnResultsScreenClosed = new();
        public static readonly EventRecorder OnResultsScreenDimCleared = new();
        public static readonly EventRecorder OnResultsScreenDimmed = new();
    }
}
