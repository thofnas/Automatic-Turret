using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy
{
    [RequireComponent(typeof(MeshRenderer))]
    public class EnemyVisual : MonoBehaviour
    {
        private const float FLASH_TWEEN_DURATION = 0.2f;
        private const float DEATH_SCALE_TWEEN_DURATION = 0.25f;
        private const float DEATH_SCALE_MULTIPLIER = 2f;
        
        private static readonly int ColorProp = Shader.PropertyToID("_EmissionColor");
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");
        private static readonly int NoiseScale = Shader.PropertyToID("_NoiseScale");
        
        [SerializeField] private Color _damagedColor;
        [SerializeField] private VisualEffect _enemyDeathVFX;

        private MeshRenderer _enemyMesh;
        private Color _originalColor;
        private MaterialPropertyBlock _materialPropertyBlock;
        
        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _enemyDeathVFX.gameObject.SetActive(false);
        }
        
        private void Start()
        {
            _enemyMesh = GetComponent<MeshRenderer>();
            _originalColor = _enemyMesh.material.GetColor(ColorProp);
        }
        
        public void ApplyFlashEffect()
        {
            _materialPropertyBlock.SetColor(ColorProp, _damagedColor);
            _enemyMesh.SetPropertyBlock(_materialPropertyBlock);

            DOVirtual.Color(_damagedColor, _originalColor, FLASH_TWEEN_DURATION, color =>
            {
                _materialPropertyBlock.SetColor(ColorProp, color);
                _enemyMesh.SetPropertyBlock(_materialPropertyBlock);
            });
        }
        
        public void StartDeathEffect()
        {
            gameObject.transform.SetParent(null);
            _enemyDeathVFX.gameObject.SetActive(true);
            transform.DOScale(transform.localScale * DEATH_SCALE_MULTIPLIER, DEATH_SCALE_TWEEN_DURATION);

            DOVirtual.Float(0.5f, 1f, DEATH_SCALE_TWEEN_DURATION, value =>
                {
                    _materialPropertyBlock.SetFloat(Opacity, value);
                    _enemyMesh.SetPropertyBlock(_materialPropertyBlock);
                })
                .SetEase(Ease.InSine);

            DOVirtual.Float(1f, 0f, DEATH_SCALE_TWEEN_DURATION, value =>
                {
                    _materialPropertyBlock.SetFloat(NoiseScale, value);
                    _enemyMesh.SetPropertyBlock(_materialPropertyBlock);
                })
                .SetEase(Ease.InSine)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
