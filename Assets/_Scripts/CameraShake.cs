using System.Collections;
using Cinemachine;
using CustomEventArgs;
using Events;
using Managers;
using Turret;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private const float SHAKE_TIME = 0.25f;
    private const float INTENSITY_MAX = 3f;
    
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

    private float _intensity;
    
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _multiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        GameEvents.TurretOnShoot.AddListener(GameEvents_Turret_OnShoot);
        GameEvents.OnEnemyDamaged.AddListener(GameEvents_Enemy_OnDamaged);
        GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnDestroyed);
    }

    private void OnDestroy()
    {
        GameEvents.TurretOnShoot.RemoveListener(GameEvents_Turret_OnShoot);
        GameEvents.OnEnemyDamaged.RemoveListener(GameEvents_Enemy_OnDamaged);
        GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnDestroyed);
    }

    private IEnumerator ShakeCamera(float intensity)
    {
        _intensity = Mathf.Clamp(_intensity + intensity, 0f, INTENSITY_MAX);
        _multiChannelPerlin.m_AmplitudeGain = _intensity;

        float elapsedTime = 0;

        while (elapsedTime < SHAKE_TIME)
        {
            float t = elapsedTime / SHAKE_TIME;

            _multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_intensity, 0f, t);
            _intensity = Mathf.Lerp(_intensity, 0f, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _multiChannelPerlin.m_AmplitudeGain = 0;
    }

    private void GameEvents_Enemy_OnDamaged(OnEnemyDamagedEventArgs onEnemyDamagedEventArgs)
    {
        StartCoroutine(ShakeCamera(onEnemyDamagedEventArgs.DealtDamage * 0.1f));
    }
    
    private void GameEvents_Enemy_OnDestroyed(Enemy.Enemy enemy)
    {
        StartCoroutine(ShakeCamera(enemy.MaxHealth * 0.25f));
    }
    
    private void GameEvents_Turret_OnShoot(Transform turret)
    {
        StartCoroutine(ShakeCamera(UpgradeManager.Instance.GetTurretUpgradedStat(Stat.BulletSpeed) * 0.5f));
    }
}
