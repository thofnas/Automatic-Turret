using Managers;
using Turret;
using Turret.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;
using UserInterface;
using UserInterface.StateMachine;
using Waves.StateMachine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TurretStateMachine _turretStateMachine;
    [SerializeField] private UIStateMachine _uiStateMachine;
    [SerializeField] private TurretShootProjectiles _turretShootProjectiles;
    [SerializeField] private WaveStateMachine _waveStateMachine;

    // execution order
    private void Awake()
    {
        _waveStateMachine.Initialize();
        _turretStateMachine.Initialize();
        _uiStateMachine.Initialize();
        _gameManager.Initialize();
        _enemyManager.Initialize();
        _turretShootProjectiles.Initialize();
    }
}
