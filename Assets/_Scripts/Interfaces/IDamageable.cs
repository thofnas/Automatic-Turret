using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        void ApplyDamage(float damage);
        Transform GetTransform();
    }
}
