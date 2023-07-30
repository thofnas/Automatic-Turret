using System;
using Managers;
using Turret.StateMachine;
using UnityEngine;
using Waves.StateMachine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private TurretStateMachine _turretStateMachine;
    [SerializeField] private WaveStateMachine _waveStateMachine;

    private void Awake()
    {
        _turretStateMachine.Initialize();
        _waveStateMachine.Initialize();
    }
}
