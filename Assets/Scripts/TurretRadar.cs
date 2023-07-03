using System;
using UnityEngine;

public class TurretRadar : MonoBehaviour
{
    public EventHandler OnEnemySpotted;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Turret.Instance.EnemyList.Enqueue(other.transform);
            OnEnemySpotted?.Invoke(this, EventArgs.Empty);
        }
    }
}
