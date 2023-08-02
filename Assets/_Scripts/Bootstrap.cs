using Managers;
using Turret;
using Turret.StateMachine;
using UnityEngine;
using UserInterface;
using Waves.StateMachine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private TurretStateMachine _turretStateMachine;
    [SerializeField] private TurretShootProjectiles _turretShootProjectiles;
    [SerializeField] private WaveStateMachine _waveStateMachine;
    [SerializeField] private LobbyUI _lobbyUI;
    [SerializeField] private GameUI _gameUI;

    // execution order
    private void Awake()
    {
        _waveStateMachine.Initialize();
        _turretStateMachine.Initialize();
        _enemyManager.Initialize();
        _turretShootProjectiles.Initialize();
        _lobbyUI.Initialize();
        _gameUI.Initialize();}
}
