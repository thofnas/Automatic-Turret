using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        void ApplyDamage();
        Transform GetTransform();
    }
}
