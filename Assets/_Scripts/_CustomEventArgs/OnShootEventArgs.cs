using System;
using UnityEngine;

namespace _CustomEventArgs
{
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 GunStartPointPosition;
        public Vector3 GunEndPointPosition;
    }
}
