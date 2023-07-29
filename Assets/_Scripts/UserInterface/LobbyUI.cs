using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace _UserInterface
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button _startWaveButton;

        private void OnEnable()
        {
            _startWaveButton.onClick.AddListener(() => UIEvents.OnStartWaveButtonClick.Invoke());
        }

        private void OnDisable()
        {
            _startWaveButton.onClick.RemoveAllListeners();
        }
    }
}
