using Events;
using UnityEngine;

namespace UserInterface.Animations
{
    [RequireComponent(typeof(Animator))]
    public class DimClearScreenAnimations : MonoBehaviour
    {
        private static readonly int DimWon = Animator.StringToHash("DimWon");
        private static readonly int DimLost = Animator.StringToHash("DimLost");
        private static readonly int Clear = Animator.StringToHash("Clear");
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameEvents.OnWaveWon.AddListener(GameEvents_Wave_OnWon);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
            //GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
            UIEvents.OnReturnToLobbyButtonClicked.AddListener(UIEvents_OnReturnToLobbyButtonClicked);
        }

        private void OnDisable()
        {
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Wave_OnWon);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Wave_OnLost);
            //GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnEnded);
            UIEvents.OnReturnToLobbyButtonClicked.RemoveListener(UIEvents_OnReturnToLobbyButtonClicked);
        }

        private void GameEvents_Wave_OnWon() => _animator.SetTrigger(DimWon);

        private void GameEvents_Wave_OnLost() => _animator.SetTrigger(DimLost);

        private void UIEvents_OnReturnToLobbyButtonClicked() => _animator.SetTrigger(Clear);
        
        private void ScreenDimmedAnimationTrigger() => UIEvents.OnResultsScreenDimmed.Invoke();
        
        private void ScreenDimClearedAnimationTrigger() => UIEvents.OnResultsScreenDimCleared.Invoke();
    }
}
