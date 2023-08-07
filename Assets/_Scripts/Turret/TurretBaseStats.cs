using System;
using UnityEngine;

namespace Turret
{
    //[CreateAssetMenu(menuName = "TurretBase")]
    public class TurretBaseStats : ScriptableObject
    {
        [Serializable] public class BaseStats : SerializableDictionary<Stat, float> {}
        public BaseStats TurretBaseStatsDictionary = new();
    }
}
