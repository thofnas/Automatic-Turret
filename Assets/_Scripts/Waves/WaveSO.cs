using System.Collections.Generic;
using UnityEngine;

namespace Waves
{
    [CreateAssetMenu(fileName = "Wave", menuName = "New Wave")]
    public class WaveSO : ScriptableObject
    {
        public List<SubWave> SubWaves;
    }

    [System.Serializable]
    public class SubWave
    {
        public List<EnemyData> EnemiesData;
        [Min(0.01F)] public float SpawnDelay = 1;
    }

    [System.Serializable]
    public class EnemyData
    {
        public Enemy.Enemy EnemyPrefab;
        [Min(1)] public int EnemyQuantity = 1;
    }
}
