using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Events;
using UnityEngine;

namespace Waves.StateMachine.States
{
    public class WaitingToFinishWaveState : WaveState
    {
        public WaitingToFinishWaveState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        private const float TIME_TO_WAIT_FOR_CLOSE_ANIMATION_END = 5f;

        private IEnumerator _proceedToLobbyRoutine;
        private TweenerCore<float, float, FloatOptions> _slowMo;
        
        public override void EnterState()
        {
            const float timeScaleStopDuration = 0.5f;
            _slowMo = DOTween.To(() => Time.timeScale, x => Time.timeScale = x,  0f, timeScaleStopDuration).SetEase(Ease.Linear);

            Debug.Log("Waiting to finish");

            UIEvents.OnResultsScreenClosed.AddListener(UIEvents_Results_OnScreenClosed);
            UIEvents.OnReturnToLobbyButtonClicked.AddListener(UIEvents_Results_OnReturnToLobbyButtonClicked);
            
            _proceedToLobbyRoutine = ProceedToLobbyRoutine();
        }

        public override void ExitState()
        {
            _slowMo.Kill();
            Time.timeScale = 0;
            GameEvents.OnWaveEnded.Invoke();
            UIEvents.OnResultsScreenClosed.RemoveListener(UIEvents_Results_OnScreenClosed);
            UIEvents.OnReturnToLobbyButtonClicked.AddListener(UIEvents_Results_OnReturnToLobbyButtonClicked);
            if (Ctx != null) Ctx.StopCoroutine(_proceedToLobbyRoutine);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }

        // switch state if results ui didn't close for some reason
        private IEnumerator ProceedToLobbyRoutine()
        {
            yield return new WaitForSeconds(TIME_TO_WAIT_FOR_CLOSE_ANIMATION_END);
            
            SwitchState(Factory.WaitingToStartWave());
            
            Debug.LogWarning("Results UI took too long to close.");
        }

        private void UIEvents_Results_OnScreenClosed() => SwitchState(Factory.WaitingToStartWave());
        
        private void UIEvents_Results_OnReturnToLobbyButtonClicked()
        {
            if (Ctx != null) 
                Ctx.StartCoroutine(_proceedToLobbyRoutine);
        }
    }
}
