using UnityEngine;

namespace _Managers
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private Wave[] _waves;
    }

    [System.Serializable]
    public class Wave
    {
        [SerializeField] private Enemy[] _enemies;
    }
}
