using System;
using Events;
using UnityEngine;

namespace Managers
{
    public class CursorManager : Singleton<CursorManager>
    {
        [Serializable] public class GameSounds : SerializableDictionary<Cursors, CursorData> {}
        [SerializeField] private GameSounds _gameSoundsDictionary = new();

        private void Start()
        {
            SetGameCursor(Cursors.Basic);
            
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
        }

        private void OnDestroy()
        {
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
        }

        public void SetGameCursor(Cursors cursor)
        {
            if (!_gameSoundsDictionary.TryGetValue(cursor, out CursorData data))
            {
                Debug.LogWarning($"Dictionary has no data about Cursor {cursor}.");
                return;
            }
            
            Cursor.SetCursor(data.Texture, data.Hotspot, CursorMode.Auto);
        }

        private void GameEvents_Wave_OnStarted() => SetGameCursor(Cursors.InBattle);
        
        private void GameEvents_Wave_OnEnded() => SetGameCursor(Cursors.Basic);
    }

    [Serializable]
    public class CursorData
    {
        public Texture2D Texture;
        public Vector2 Hotspot;
    }

    public enum Cursors
    {
        Basic,
        InBattle,
        ButtonHover
    }
}
