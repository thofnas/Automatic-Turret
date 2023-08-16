using System;

namespace CustomEventArgs
{
    public class OnEnemyDamagedEventArgs : EventArgs
    {
        public Enemy.Enemy Enemy;
        public float DealtDamage;
    }
}
