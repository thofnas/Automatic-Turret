using Events;
using UnityEngine;

namespace UserInterface.StateMachine.States
{
    public class UIResultsState : UIState
    {
        public UIResultsState(UIStateMachine currentContext, UIStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }

        public override void EnterState()
        {
            PrepareElementsForAnimation();
            UIEvents.OnResultsScreenClosed.AddListener(UIEvents_Results_OnScreenClosed);
            Ctx.ReturnToLobbyButton.onClick.AddListener(() => UIEvents.OnReturnToLobbyButtonClicked.Invoke());
        }

        public override void ExitState()
        {
            UIEvents.OnResultsScreenClosed.RemoveListener(UIEvents_Results_OnScreenClosed);
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
            Ctx.WaveResultsParentScreenUITransform.gameObject.SetActive(false);
            Ctx.WaveLostScreenUITransform.gameObject.SetActive(false);
            Ctx.WaveWonScreenUITransform.gameObject.SetActive(false);
        }
        
        public void PrepareElementsForAnimation()
        {
            // Utilities.TryGetComponentInChildren(ReturnToLobbyButton.gameObject, out TextMeshProUGUI btnText, true);
            // Color btnColor = ReturnToLobbyButton.GetComponent<Image>().color;
            // btnColor = new Color(btnColor.r, btnColor.g, btnColor.b, Color.clear.a);
            // ReturnToLobbyButton.GetComponent<Image>().color = btnColor;
            // btnText.color = Color.clear;
            Ctx.WaveWonText.color = Color.clear;
            Ctx.WaveLostText.color = Color.clear;
        }
        
        private void UIEvents_Results_OnScreenClosed() => SwitchState(Factory.UILobby());
    }
}
