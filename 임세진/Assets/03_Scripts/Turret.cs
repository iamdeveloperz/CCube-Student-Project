using UnityEngine;

public class Turret : MonoBehaviour
{
    // 좀비를 탐색하는 거리
    public float range = 8f;

    // 터렛의 Head
    public Transform head;

    // 총알이 생성되는 위치
    public Transform firePoint;

    // 터렛이 발사할 총알 프리팹
    public GameObject bulletPrefab;

    // 초당 발사 횟수
    public float fireRate = 2f;

    private float fireTimer = 0f;

    // 현재 공격할 좀비
    private Transform target;

    void Update()
    {
        // 가장 가까운 좀비 찾기
        FindTarget();

        if (target != null)
        {
            // Head가 좀비를 바라봄
            RotateToTarget();

            // 발사 시간 계산
            fireTimer += Time.deltaTime;

            if (fireTimer >= 1f / fireRate)
            {
                Shoot();

                fireTimer = 0f;
            }
        }
        else
        {
            // 좀비가 없으면 발사 타이머 초기화
            fireTimer = 0f;
        }
    }

    void FindTarget()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        float shortestDistance = Mathf.Infinity;
        Transform nearestZombie = null;

        foreach (GameObject zombie in zombies)
        {
            float distance = Vector3.Distance(
                transform.position,
                zombie.transform.position
            );

            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearestZombie = zombie.transform;
            }
        }

        target = nearestZombie;
    }

    void RotateToTarget()
    {
        Vector3 direction = target.position - head.position;

        // 위아래로는 회전하지 않음
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            head.rotation = Quaternion.Lerp(
                head.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }

    void Shoot()
    {
        // FirePoint 위치와 방향으로 총알 생성
        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        // 생성된 총알의 TurretBullet 가져오기
        TurretBullet bullet =
            bulletObject.GetComponent<TurretBullet>();

        // 총알에게 현재 공격할 좀비 전달
        if (bullet != null)
        {
            bullet.SetTarget(target);
        }
    }
}