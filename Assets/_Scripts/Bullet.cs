using Interfaces;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float BULLET_SPEED = 10f;
    private const float LIFE_TIME = 3f;
    private Vector3 _shootDir;

    private void Awake() => Destroy(gameObject, LIFE_TIME);

    private void Update() => transform.position += _shootDir * (Time.deltaTime * BULLET_SPEED);

    public void Setup(Vector3 shootDir) => _shootDir = shootDir;

    private void OnTriggerEnter(Collider bulletCollider)
    {
        var damageable = bulletCollider.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.ApplyDamage();
            Destroy(gameObject);
        }
    }
}

