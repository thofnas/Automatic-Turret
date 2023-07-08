using System;
using System.Collections.Generic;
using _CustomEventArgs;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Managers
{
    public class EnemyManager : Singleton<EnemyManager>
    { 
        public List<Enemy> EnemiesInSightList;

       private void OnEnable()
       {
           EnemiesInSightList.ForEach(SubscribeEnemyToEvents);
       }

       private void SubscribeEnemyToEvents(Enemy enemy)
       {
           enemy.OnEnemyDestroyEvent -= Enemy_OnDestroyEvent;
       }

       private void UnsubscribeEnemyFromEvents(Enemy enemy)
       {
           enemy.OnEnemyDestroyEvent -= Enemy_OnDestroyEvent;
       }
       
       private void Enemy_OnDestroyEvent(object sender, OnEnemyDestroyEventArgs e)
       {
           EnemiesInSightList.Remove(e.Enemy);
           UnsubscribeEnemyFromEvents(e.Enemy);
       }
       
       public void AddEnemy(Enemy enemy)
       {
           EnemiesInSightList.Add(enemy);
           SubscribeEnemyToEvents(enemy);
       }
    }
}
