using CustomEventArgs;
using DG.Tweening;
using Events;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(MeshRenderer))]
    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private Color _damagedColor;

        private const float ANIMATION_DURATION = 0.1f;
        private MeshRenderer _enemyMesh;
        private MaterialPropertyBlock _materialPropertyBlock;
        
        private void Start()
        {
            _enemyMesh = GetComponent<MeshRenderer>();
            GameEvents.OnEnemyDamaged.AddListener(GameEvents_Enemy_OnDamaged);
        }

        private void OnDestroy()
        {
            GameEvents.OnEnemyDamaged.RemoveListener(GameEvents_Enemy_OnDamaged);
        }

        private void GameEvents_Enemy_OnDamaged(OnEnemyDamagedEventArgs onEnemyDamagedEventArgs)
        {
            _enemyMesh.material.DOColor(_damagedColor, ANIMATION_DURATION).SetLoops(1);
        }
    }
}
