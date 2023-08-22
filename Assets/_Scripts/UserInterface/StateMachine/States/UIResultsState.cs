using Events;
using Managers;

namespace UserInterface.StateMachine.States
{
    public class UIResultsState : UIState
    {
        public UIResultsState(UIStateMachine currentContext, UIStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }

        public override void EnterState()
        {
            UIEvents.OnResultsScreenClosed.AddListener(UIEvents_Results_OnScreenClosed);
            GameEvents.OnCollectedGearAmountChanged.AddListener(GameEvents_OnCollectedGearAmountChanged);
            Ctx.ReturnToLobbyButton.onClick.AddListener(() =>
            {
                UIEvents.OnReturnToLobbyButtonClicked.Invoke();
            });
            Ctx.CollectedGearsAmountInResults.text = GameManager.Instance.CollectedGearAmount.ToString();
        }

        public override void ExitState()
        {
            UIEvents.OnResultsScreenClosed.RemoveListener(UIEvents_Results_OnScreenClosed);
            GameEvents.OnCollectedGearAmountChanged.RemoveListener(GameEvents_OnCollectedGearAmountChanged);
            Ctx.ReturnToLobbyButton.onClick.RemoveAllListeners();
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            
            if (Ctx.ReturnToLobbyButton.gameObject != null && !Ctx.ReturnToLobbyButton.gameObject.activeSelf)
                Ctx.ReturnToLobbyButton.gameObject.SetActive(true);
        }

        public override void CheckSwitchStates() { }

        public override void EnableElement()
        {
            Ctx.WaveResultsParentScreenUITransform.gameObject.SetActive(true);
        }
        
        public override void DisableElement()
        {
            if (Ctx == null) return;

            Ctx.WaveResultsParentScreenUITransform.gameObject.SetActive(false);
            Ctx.WaveLostScreenUITransform.gameObject.SetActive(false);
            Ctx.WaveWonScreenUITransform.gameObject.SetActive(false);
        }

        private void GameEvents_OnCollectedGearAmountChanged(int collectedGearAmount)
        {
            Ctx.CollectedGearsAmountInResults.text = collectedGearAmount.ToString();
        }

        private void UIEvents_Results_OnScreenClosed() => SwitchState(Factory.UILobby());
    }
}
