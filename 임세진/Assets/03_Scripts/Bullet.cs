using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            ZombieAI zombie = other.GetComponent<ZombieAI>();

            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }

        // 벽이나 좀비를 맞으면 총알 제거
        if (other.CompareTag("Zombie") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}