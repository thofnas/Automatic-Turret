using DG.Tweening;
using Events;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        public void PickUp()
        {
            transform.DOKill();
            GameEvents.OnItemPicked.Invoke();
            Destroy(gameObject);
        }
    }
}