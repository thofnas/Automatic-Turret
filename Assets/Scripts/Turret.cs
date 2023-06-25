using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform _gunStartPoint;
    [SerializeField] private Transform _gunEndPoint;
    
    private float _shootTimer;
    private const float SHOOT_TIMER_MAX = 1f;
    public event EventHandler<OnShootEventArgs> OnShoot;

    private void Update()
    {
        _shootTimer += Time.deltaTime;
        if (!(_shootTimer >= SHOOT_TIMER_MAX)) return;
        
        Shoot();
        _shootTimer = 0;
    }

    public void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs {
            GunEndPointPosition = _gunEndPoint.position,
            GunStartPointPosition = _gunStartPoint.position
        });
    }
    
    
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 GunStartPointPosition;
        public Vector3 GunEndPointPosition;
    }
}
