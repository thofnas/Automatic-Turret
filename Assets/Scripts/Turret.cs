using System;
using System.Collections;
using CustomEventArgs;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public event EventHandler OnReloadStart; 
    public event EventHandler OnReloadEnd;
    
    [SerializeField] private Transform _gunStartPoint;
    [SerializeField] private Transform _gunEndPoint;
    [SerializeField, Range(0.1F, 4F)] private float _reloadTimeInSeconds;
    
    private bool _isReloading = true;

    private void Update()
    {
        if (_isReloading) return;
        
        Shoot();
    }

    public void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs {
            GunEndPointPosition = _gunEndPoint.position,
            GunStartPointPosition = _gunStartPoint.position,
        });

        StartCoroutine(ReloadGunRoutine());
    }
    
    private IEnumerator ReloadGunRoutine()
    {
        _isReloading = true;
        
        OnReloadStart?.Invoke(this, EventArgs.Empty);
        
        yield return new WaitForSeconds(_reloadTimeInSeconds);
        
        _isReloading = false;
        
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }
}
