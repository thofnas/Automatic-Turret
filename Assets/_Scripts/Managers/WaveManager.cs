using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Waves;

namespace Managers
{
    public class WaveManager : Singleton<WaveManager>
    {
        [SerializeField] private List<WaveSO> _waves;
        
        public int CurrentWave { get; private set; }
        
        public IEnumerable<WaveSO> GetWaves() => _waves;
    }
}
