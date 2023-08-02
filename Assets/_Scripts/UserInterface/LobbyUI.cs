using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button _startWaveButton;

        public void Initialize()
        {
            _startWaveButton.onClick.AddListener(() => UIEvents.OnStartWaveButtonClick.Invoke());
        }

        private void OnDestroy()
        {
            _startWaveButton.onClick.RemoveAllListeners();
        }
    }
}
