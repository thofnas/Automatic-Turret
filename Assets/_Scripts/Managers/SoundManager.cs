using System;
using System.Collections.Generic;
using CustomEventArgs;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Serializable] public class GameSounds : SerializableDictionary<Sounds, AudioClips> {}
        [SerializeField] private GameSounds _gameSoundsDictionary = new();

        private GameObject _playOneShot;
        private AudioSource _audioSource;

        public void Initialize()
        {
            GameEvents.OnTurretShoot.AddListener(GameEvents_Turret_OnShoot);
            GameEvents.OnEnemyDamaged.AddListener(GameEvents_Enemy_OnDamaged);
            GameEvents.OnItemPicked.AddListener(GameEvents_Item_OnPicked);
            GameEvents.OnEnemySpawned.AddListener(GameEvents_Enemy_OnSpawned);
        }

        private void OnDestroy()
        {
            GameEvents.OnTurretShoot.RemoveListener(GameEvents_Turret_OnShoot);
            GameEvents.OnEnemyDamaged.RemoveListener(GameEvents_Enemy_OnDamaged);
            GameEvents.OnItemPicked.RemoveListener(GameEvents_Item_OnPicked);
            GameEvents.OnEnemySpawned.RemoveListener(GameEvents_Enemy_OnSpawned);
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
            if (_playOneShot == null)
            {
                _playOneShot = new GameObject("Sound");
                _audioSource = _playOneShot.AddComponent<AudioSource>();
            }

            volume = Mathf.Clamp(volume, 0f, 1f);

            _audioSource.PlayOneShot(GetAudioClip(sound), volume);
        }
        
        private void PlaySound(Sounds sound, Vector3 position, float volume = 1f)
        {
            if (_playOneShot == null)
            {
                _playOneShot = new GameObject("Sound");
                _audioSource = _playOneShot.AddComponent<AudioSource>();
            }
            
            _playOneShot.transform.position = position;

            volume = Mathf.Clamp(volume, 0f, 1f);

            _audioSource.clip = GetAudioClip(sound);
            _audioSource.volume = volume;
            _audioSource.Play();
        }
        
        
        private void GameEvents_Turret_OnShoot(Transform turret) => PlaySound(Sounds.TurretShoot);

        private void GameEvents_Enemy_OnDamaged(OnEnemyDamagedEventArgs obj) => PlaySound(Sounds.EnemyDamaged, obj.Enemy.transform.position, 0.2f);
        
        private void GameEvents_Item_OnPicked() => PlaySound(Sounds.ItemPicked, 0.5f);

        private void GameEvents_Enemy_OnSpawned(Enemy.Enemy enemy) => PlaySound(Sounds.EnemySpawned, enemy.transform.position, 0.5f);
    }

    [Serializable]
    public class AudioClips
    {
        public List<AudioClip> Clips;
    }

    public enum Sounds
    {
        TurretShoot,
        EnemyDamaged,
        EnemyDestroyed,
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
