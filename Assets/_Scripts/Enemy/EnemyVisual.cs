using DG.Tweening;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(MeshRenderer))]
    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private Color _damagedColor;

        private const float ANIMATION_DURATION = 0.2f;
        private MeshRenderer _enemyMesh;
        private Color _originalColor;
        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int ColorProp = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
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

            DOVirtual.Color(_damagedColor, _originalColor, ANIMATION_DURATION, color =>
            {
                _materialPropertyBlock.SetColor(ColorProp, color);
                _enemyMesh.SetPropertyBlock(_materialPropertyBlock);
            });
        }
    }
}
