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
            GameEvents.OnGearAmountChanged.AddListener(GameEvents_Item_OnGearAmountChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
        }

        public override void ExitState() {
            Ctx.StartWaveButton.onClick.RemoveAllListeners();
            GameEvents.OnGearAmountChanged.RemoveListener(GameEvents_Item_OnGearAmountChanged);
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }
        
        public override void EnableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(true);
        
        public override void DisableElement() => Ctx.LobbyScreenUITransform.gameObject.SetActive(false);

        private void GameEvents_Item_OnGearAmountChanged()
        {
            Ctx.TotalGearsCount.text = GameManager.Instance.TotalGearCount.ToString();
        }
        
        private void GameEvents_Wave_OnWaveStarted()
        {
            SwitchState(Factory.UIGame());
        }
    }
}
