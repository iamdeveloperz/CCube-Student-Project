using UnityEngine;
using UnityEngine.AI;

// 귀신(Enemy)의 이동 속도에 맞춰 대기 / 걷기 / 달리기 애니메이션을 재생합니다.
// Enemy.cs 는 수정하지 않고 NavMeshAgent 의 실제 속도만 읽어 옵니다.
public class GhostAnimator : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    // 애니메이션 전환을 부드럽게
    public float dampTime = 0.15f;

    void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || agent == null)
            return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed, dampTime, Time.deltaTime);
    }
}
