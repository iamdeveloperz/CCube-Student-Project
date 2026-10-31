using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float speed = 2f;
    public int hp = 100;

    private int currentWaypoint = 1;

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        // 먼저 범위를 확인
        if (currentWaypoint >= WayPointManager.instance.waypoints.Length)
        {
            BaseManager.instance.TakeDamage(10);

            Destroy(gameObject);
            return;
        }

        Transform target = WayPointManager.instance.waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint++;
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GoldManager.instance.AddGold(10);

        Destroy(gameObject);
    }
}