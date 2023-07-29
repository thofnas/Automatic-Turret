using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Waves
{
    [CreateAssetMenu(fileName = "Wave", menuName = "New Wave")]
    public class WaveSO : ScriptableObject
    {
        public List<EnemyData> EnemiesData;
        [Min(0.01F)] public float SpawnDelay;
    }

    [System.Serializable]
    public class EnemyData
    {
        public Enemy EnemyPrefab;
        [Min(1)] public int EnemyQuantity;
    }
}
