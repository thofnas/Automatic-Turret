using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public void Damage()
    {
        Destroy(gameObject);
    }
}
