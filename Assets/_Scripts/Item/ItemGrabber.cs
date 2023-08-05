using UnityEngine;

namespace Item
{
    public class ItemGrabber : MonoBehaviour
    {
        [SerializeField] private LayerMask _grabbableLayerMask;
        [SerializeField, Min(1f)] private float _radius;

        private void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _grabbableLayerMask);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out global::Item.Item item))
                {
                    item.PickUp();
                }
            }
        }
    }
}
