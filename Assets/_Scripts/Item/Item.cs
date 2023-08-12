using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.4f;
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        [SerializeField] private List<MeshRenderer> _gearMeshes;
        private readonly List<Material> _gearMaterials = new();
        private bool _isPicked;
        private bool _areMaterialsLoaded;

        private void Start()
        {
            foreach (Material material in _gearMeshes.SelectMany(gearMesh => gearMesh.materials))
            {
                if (material == null) continue;
                _gearMaterials.Add(material);
            }

            _areMaterialsLoaded = true;
        }

        public void PickUp()
        {
            if (_isPicked || !_areMaterialsLoaded) return;
            _isPicked = true;
            
            transform.DOKill();
            
            GameEvents.OnItemPicked.Invoke();

            transform.DOMoveY(Vector3.up.y, ANIMATION_DURATION)
                .SetEase(Ease.OutSine)
                .OnComplete(() => Destroy(gameObject));

            _gearMaterials.ForEach(gearMaterial =>
            {
                gearMaterial.DOFloat(1f, Opacity, ANIMATION_DURATION)
                    .SetEase(Ease.OutSine);
            });
        }
    }
}