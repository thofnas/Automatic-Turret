using Events;
using Managers;

namespace UserInterface.StateMachine.States
{
    public class UILobbyScreenState : UIBaseState
    {
        public UILobbyScreenState(UIStateMachine context, UIStateFactory uiStateFactory)
            : base(context, uiStateFactory) { }
        
        public override void EnterState() {
            Ctx.StartWaveButton.onClick.AddListener(() => UIEvents.OnStartWaveButtonClicked.Invoke());
            GameEvents.OnTotalGearAmountChanged.AddListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
        }

        public override void ExitState() {
            Ctx.StartWaveButton.onClick.RemoveAllListeners();
            GameEvents.OnTotalGearAmountChanged.RemoveListener(GameEvents_Item_OnTotalGearAmountChanged);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnWaveEnded);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }
        
        public override void EnableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(true);
        
        public override void DisableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(false);

        private void UpdateUI()
        {
            Ctx.TotalGearsCount.text = GameManager.Instance.TotalGearAmount.ToString();
        }

        private void GameEvents_Item_OnTotalGearAmountChanged() => UpdateUI();

        private void GameEvents_Wave_OnWaveEnded() => UpdateUI();

        private void GameEvents_Wave_OnWaveStarted() => SwitchState(Factory.UIGame());
    }
}
