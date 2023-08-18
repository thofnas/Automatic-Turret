using System;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.Pool;

namespace Item
{
    public class Gear : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.4f;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        [SerializeField] private MeshRenderer _gearMesh;
        private MaterialPropertyBlock _materialPropertyBlock;
        private bool _isPicked;
        private IObjectPool<Gear> _pool;
        
        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
            UIEvents.OnReturnToLobbyButtonClicked.AddListener(GameEvents_OnReturnToLobbyButtonClicked);
        }

        private void OnDisable()
        {
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Wave_OnLost);
            UIEvents.OnReturnToLobbyButtonClicked.RemoveListener(GameEvents_OnReturnToLobbyButtonClicked);
        }

        public void SetPool(IObjectPool<Gear> pool) => _pool = pool;

        public void PickUp()
        {
            if (_isPicked) return;
            _isPicked = true;

            transform.DOKill();

            GameEvents.OnItemPicked.Invoke();

            AnimateAndRelease();
        }
        
        private void AnimateAndRelease()
        {
            transform.DOMoveY(Vector3.up.y, ANIMATION_DURATION)
                .SetEase(Ease.OutSine)
                .OnComplete(() => _pool.Release(this))
                .SetUpdate(true);
            
            DOVirtual.Float(0f, 1f, ANIMATION_DURATION, opacity =>
            {
                _materialPropertyBlock.SetFloat(Opacity, opacity);
                _gearMesh.SetPropertyBlock(_materialPropertyBlock);
            }).SetEase(Ease.OutSine).SetUpdate(true);
        }

        public void ResetItemProperties()
        {
            _isPicked = false;
            _materialPropertyBlock.SetFloat(Opacity, 0f);
            _gearMesh.SetPropertyBlock(_materialPropertyBlock);
        }
        
        private void GameEvents_Wave_OnLost()
        {
            AnimateAndRelease();
        }
        
        private void GameEvents_OnReturnToLobbyButtonClicked()
        {
            PickUp();
        }
    }
}