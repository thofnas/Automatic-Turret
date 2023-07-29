using Events;
using TMPro;
using UnityEngine;

namespace Waves.StateMachine
{
    public class WaveStateMachine : MonoBehaviour
    {
        // state variables
        private WaveStateFactory _states;
        public WaveBaseState CurrentState { get; set; }

        #region Unity methods
        private void Awake()
        {
            _states = new WaveStateFactory(this);
        }

        private void Start()
        {
            CurrentState = _states.WaitingForPlayer();
            CurrentState.EnterState();
            GameEvents.OnWaveStateChanged.Invoke(CurrentState.ToString());
        }

        private void Update() => CurrentState.UpdateState();

        #endregion

        public Transform GetTransform() => transform;
    }
}
