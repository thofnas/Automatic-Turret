using DG.Tweening;
using Events;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.4f;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        [SerializeField] private MeshRenderer _gearMesh;
        private MaterialPropertyBlock _materialPropertyBlock;
        private bool _isPicked;
        private bool _areMaterialsLoaded;
        

        private void Start()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void PickUp()
        {
            if (_isPicked) return;
            _isPicked = true;

            transform.DOKill();

            GameEvents.OnItemPicked.Invoke();

            transform.DOMoveY(Vector3.up.y, ANIMATION_DURATION)
                .SetEase(Ease.OutSine)
                .OnComplete(() => GameEvents.OnItemPickUpAnimationCompleted.Invoke(this))
                .SetUpdate(true);

            DOVirtual.Float(0f, 1f, ANIMATION_DURATION, opacity =>
            {
                _materialPropertyBlock.SetFloat(Opacity, opacity);
                _gearMesh.SetPropertyBlock(_materialPropertyBlock);
            }).SetEase(Ease.OutSine).SetUpdate(true);
        }
    }
}