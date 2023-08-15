using Managers;
using Turret;
using Turret.StateMachine;
using UnityEngine;
using UserInterface.StateMachine;
using Waves.StateMachine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TurretStateMachine _turretStateMachine;
    [SerializeField] private UIStateMachine _uiStateMachine;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private TurretShootProjectiles _turretShootProjectiles;
    [SerializeField] private WaveManager _waveManager;

    // execution order
    private void Awake()
    {
        _upgradeManager.Initialize();
        _gameManager.Initialize();
        _enemyManager.Initialize();
        _waveManager.Initialize();
        _turretStateMachine.Initialize();
        _turretShootProjectiles.Initialize();
        _uiStateMachine.Initialize();
    }
}
