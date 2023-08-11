using DG.Tweening;
using Events;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Material _gearMaterial;
        
        private bool _isPicked;
        
        public void PickUp()
        {
            if (_isPicked) return;
            _isPicked = true;
            
            transform.DOKill();
            
            GameEvents.OnItemPicked.Invoke();
            
            const float animationDuration = 0.4f;
            
            transform.DOMoveY(Vector3.up.y, animationDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}