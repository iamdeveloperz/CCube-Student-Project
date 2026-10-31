using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    // 총알 속도
    public float speed = 20f;

    // 총알 대미지
    public int damage = 50;

    // 현재 추적할 좀비
    private Transform target;

    // Turret에서 타겟을 전달받음
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Start()
    {
        // 3초가 지나면 총알 삭제
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // 좀비가 죽거나 사라졌다면 총알 삭제
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 현재 총알 위치에서 좀비의 현재 위치까지의 방향
        Vector3 direction =
            target.position - transform.position;

        // 좀비를 바라봄
        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }

        // 좀비 방향으로 이동
        transform.position +=
            transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 좀비에 맞았을 때
        if (other.CompareTag("Zombie"))
        {
            ZombieAI zombie =
                other.GetComponent<ZombieAI>();

            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }

            // 좀비를 맞히면 총알 삭제
            Destroy(gameObject);
        }

        // 벽에 맞았을 때
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}