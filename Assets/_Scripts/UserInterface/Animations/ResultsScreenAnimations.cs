using Events;
using UnityEngine;

namespace UserInterface.Animations
{
    [RequireComponent(typeof(Animator))]
    public class ResultsScreenAnimations : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Close = Animator.StringToHash("Close");
        private Animator _animator;

        private void Start() => _animator = GetComponent<Animator>();

        private void OnEnable() =>
            UIEvents.OnResultsScreenDimmed.AddListener(UIEvents_Results_ScreenDimmed);

        private void OnDisable() =>
            UIEvents.OnResultsScreenDimmed.RemoveListener(UIEvents_Results_ScreenDimmed);

        private void AllowReturnButtonClicksTrigger() => 
            UIEvents.OnReturnToLobbyButtonClicked.AddListener(UIEvents_Results_ReturnToLobbyButtonClicked);

        private void DisallowReturnButtonClicks() =>
            UIEvents.OnReturnToLobbyButtonClicked.RemoveListener(UIEvents_Results_ReturnToLobbyButtonClicked);
        
        private void ResultsScreenClosed() => UIEvents.OnResultsScreenClosed.Invoke();
        
        private void UIEvents_Results_ScreenDimmed() =>
            _animator.SetTrigger(Open);

        private void UIEvents_Results_ReturnToLobbyButtonClicked()
        {
            DisallowReturnButtonClicks();
            _animator.SetTrigger(Close);
        }
    }
}
