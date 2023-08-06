using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.StateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [Header("Play Screen")]
        [SerializeField] private Transform _playScreenUITransform;
        public TextMeshProUGUI CurrentSubWaveCount;
        public TextMeshProUGUI CollectedGearsAmount;
        [Header("Lobby Screen")]
        [SerializeField] private Transform _lobbyScreenUITransform;
        public TextMeshProUGUI CurrentWaveCount;
        public TextMeshProUGUI TotalGearsCount;
        public Button StartWaveButton;
        public Button UpgradeHealthButton;
        public TextMeshProUGUI HealthLevelText;
        
        //get/set
        public Transform PlayScreenUITransform { get => _playScreenUITransform; }
        public Transform LobbyScreenUITransform { get => _lobbyScreenUITransform; }

        // state variables
        private UIStateFactory _states;
        public UIBaseState CurrentState { get; set; }
        
        public void Initialize()
        {
            _states = new UIStateFactory(this);
            
            CurrentState = _states.UILobby();
            
            CurrentState.EnterState();
        }

        #region Unity methods
        private void Update() => CurrentState.UpdateState();

        #endregion
    }
}
