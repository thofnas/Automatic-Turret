namespace Events
{
    public abstract class UIEvents
    {
        public static readonly EventRecorder OnUpdateGameUIText = new();
        public static readonly EventRecorder OnStartWaveButtonClick = new();
    }
}
