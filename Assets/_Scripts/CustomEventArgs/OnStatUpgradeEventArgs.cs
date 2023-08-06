using System;
using Turret;

namespace CustomEventArgs
{
    public class OnStatUpgradeEventArgs : EventArgs
    {
        public Stat Stat;
        public int Price;
        public float NewStatValue;
    }
}
