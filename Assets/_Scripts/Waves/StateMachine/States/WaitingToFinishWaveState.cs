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

        private IEnumerator _proceedToLobbyRoutine;
        private TweenerCore<float, float, FloatOptions> _slowMo;
        
        public override void EnterState()
        {
            const float timeScaleStopDuration = 0.5f;
            _slowMo = DOTween.To(() => Time.timeScale, x => Time.timeScale = x,  0f, timeScaleStopDuration).SetEase(Ease.Linear);

            Debug.Log("Waiting to finish");

            UIEvents.OnResultsScreenClosed.AddListener(UIEvents_Results_OnScreenClosed);
        }

        public override void ExitState()
        {
            _slowMo.Kill();
            Time.timeScale = 0;
            GameEvents.OnWaveEnded.Invoke();
            UIEvents.OnResultsScreenClosed.RemoveListener(UIEvents_Results_OnScreenClosed);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }

        private void UIEvents_Results_OnScreenClosed() => SwitchState(Factory.WaitingToStartWave());
    }
}
