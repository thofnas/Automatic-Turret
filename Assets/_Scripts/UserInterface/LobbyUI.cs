using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
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
