using System;
using System.Collections.Generic;
using CustomEventArgs;
using Events;
using Turret;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Serializable] public class GameSounds : SerializableDictionary<Sounds, AudioClips> {}
        [SerializeField] private GameSounds _gameSoundsDictionary = new();

        public void Initialize()
        {
            GameEvents.OnTurretShoot.AddListener(GameEvents_Turret_OnShoot);
            GameEvents.OnTurretDamaged.AddListener(GameEvents_Turret_OnDamaged);
            GameEvents.OnEnemyDamaged.AddListener(GameEvents_Enemy_OnDamaged);
            GameEvents.OnItemPicked.AddListener(GameEvents_Item_OnPicked);
            GameEvents.OnEnemySpawned.AddListener(GameEvents_Enemy_OnSpawned);
            GameEvents.OnEnemyKilled.AddListener(GameEvents_Enemy_OnKilled);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            UIEvents.OnUpgradeButtonClicked.AddListener(UIEvents_OnButtonClicked);
            UIEvents.OnResetUpgradesButtonClicked.AddListener(UIEvents_OnButtonClicked);
            UIEvents.OnStartWaveButtonClicked.AddListener(UIEvents_OnButtonClicked);
            UIEvents.OnReturnToLobbyButtonClicked.AddListener(UIEvents_OnButtonClicked);
        }

        private void OnDestroy()
        {
            GameEvents.OnTurretShoot.RemoveListener(GameEvents_Turret_OnShoot);
            GameEvents.OnTurretDamaged.AddListener(GameEvents_Turret_OnDamaged);
            GameEvents.OnEnemyDamaged.RemoveListener(GameEvents_Enemy_OnDamaged);
            GameEvents.OnItemPicked.RemoveListener(GameEvents_Item_OnPicked);
            GameEvents.OnEnemySpawned.RemoveListener(GameEvents_Enemy_OnSpawned);
            GameEvents.OnEnemyKilled.RemoveListener(GameEvents_Enemy_OnKilled);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            UIEvents.OnUpgradeButtonClicked.RemoveListener(UIEvents_OnButtonClicked);
            UIEvents.OnResetUpgradesButtonClicked.RemoveListener(UIEvents_OnButtonClicked);
            UIEvents.OnStartWaveButtonClicked.RemoveListener(UIEvents_OnButtonClicked);
            UIEvents.OnReturnToLobbyButtonClicked.RemoveListener(UIEvents_OnButtonClicked);
        }

        private AudioClip GetAudioClip(Sounds sound)
        {
            if (_gameSoundsDictionary.TryGetValue(sound, out AudioClips audioClips)) 
                return audioClips.Clips[Random.Range(0, audioClips.Clips.Count)];
            
            Debug.LogError($"Sound {sound} not found.");
            return null;
        }
    
        private void PlaySound(Sounds sound, float volume = 1f)
        {
            var oneShotGameObject = new GameObject("Sound");
            var audioSource = oneShotGameObject.AddComponent<AudioSource>();

            volume = Mathf.Clamp(volume, 0f, 1f);

            audioSource.clip = GetAudioClip(sound);
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(oneShotGameObject, audioSource.clip.length);
        }
        
        private void PlaySound(Sounds sound, Vector3 position, float volume = 1f)
        {
            var oneShotGameObject = new GameObject("Sound");
            var audioSource = oneShotGameObject.AddComponent<AudioSource>();

            oneShotGameObject.transform.position = position;

            volume = Mathf.Clamp(volume, 0f, 1f);

            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 0.5f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(oneShotGameObject, audioSource.clip.length);
        }

        private void GameEvents_Turret_OnShoot(Transform turret) => PlaySound(Sounds.TurretShoot);

        private void GameEvents_Enemy_OnDamaged(OnEnemyDamagedEventArgs obj) => PlaySound(Sounds.EnemyDamaged, obj.Enemy.transform.position);
        
        private void GameEvents_Item_OnPicked() => PlaySound(Sounds.ItemPicked, 0.6f);

        private void GameEvents_Enemy_OnSpawned(Enemy.Enemy enemy) => PlaySound(Sounds.EnemySpawned, enemy.transform.position, 0.3f);
        
        private void GameEvents_Enemy_OnKilled(Enemy.Enemy enemy) => PlaySound(Sounds.EnemyKilled, enemy.transform.position, 0.5f);

        private void GameEvents_Turret_OnDamaged() => PlaySound(Sounds.TurretDamaged, 0.2f);
    
        private void GameEvents_Wave_OnStarted() => PlaySound(Sounds.WaveStarted, 0.2f);
        
        private void UIEvents_OnButtonClicked(Stat stat) => PlaySound(Sounds.ButtonClick);

        private void UIEvents_OnButtonClicked() => PlaySound(Sounds.ButtonClick);
    }
    

    [Serializable]
    public class AudioClips
    {
        public List<AudioClip> Clips;
    }

    public enum Sounds
    {
        TurretShoot,
        TurretDamaged,
        EnemyDamaged,
        EnemyKilled,
        ItemPicked,
        EnemySpawned,
        ButtonClick,
        WaveStarted,
        WaveLost,
        WaveWon,
        UITransition,
        StatUpgraded,
        StatsReset,
        TurretRotationStart,
        TurretRotationLoop,
        TurretRotationEnd
    }
}
