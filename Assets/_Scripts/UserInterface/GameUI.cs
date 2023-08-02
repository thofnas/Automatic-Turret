using Events;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UserInterface
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _stateTestText;
        [SerializeField] private TextMeshProUGUI _currentWaveCount;
        [SerializeField] private TextMeshProUGUI _currentSubWaveCount;

        public void Initialize()
        {
            GameEvents.OnWaveStateChanged.AddListener(GameEvents_Waves_OnWaveStateChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnSubWaveStarted.AddListener(GameEvents_Wave_OnSubWaveStarted);
            GameEvents.OnSubWaveEnded.AddListener(GameEvents_Wave_OnSubWaveEnded);
        }

        private void OnDestroy()
        {
            GameEvents.OnWaveStateChanged.RemoveListener(GameEvents_Waves_OnWaveStateChanged);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnSubWaveStarted.AddListener(GameEvents_Wave_OnSubWaveStarted);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Wave_OnSubWaveEnded);
        }

        private void UpdateGameUIText()
        {
            _currentWaveCount.text = GameManager.Instance.WaveStateMachine.CurrentWaveID == 0 
                ? "Tutorial" 
                : GameManager.Instance.WaveStateMachine.CurrentWaveID.ToString();

            _currentSubWaveCount.text 
                = $"{GameManager.Instance.WaveStateMachine.CurrentSubWaveID + 1} / {GameManager.Instance.WaveStateMachine.CurrentSubWaveIDMax + 1}";
        }
        
        private void GameEvents_Waves_OnWaveStateChanged(string str) => _stateTestText.text = str.ToSafeString();

        private void GameEvents_Wave_OnWaveStarted() => UpdateGameUIText();

        private void GameEvents_Wave_OnWaveEnded() => UpdateGameUIText();

        private void GameEvents_Wave_OnSubWaveStarted() => UpdateGameUIText();

        private void GameEvents_Wave_OnSubWaveEnded() => UpdateGameUIText();
    }
}
