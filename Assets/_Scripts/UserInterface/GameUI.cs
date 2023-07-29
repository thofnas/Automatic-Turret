using System;
using Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Waves.StateMachine;

namespace _UserInterface
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _stateTestText;

        private void OnEnable()
        {
            GameEvents.OnWaveStateChanged.AddListener(GameEvents_Waves_OnWaveStateChanged);
        }

        private void OnDisable()
        {
            GameEvents.OnWaveStateChanged.RemoveListener(GameEvents_Waves_OnWaveStateChanged);
        }
        
        private void GameEvents_Waves_OnWaveStateChanged(string str)
        {
            _stateTestText.text = str.ToSafeString();
        }
    }
}
