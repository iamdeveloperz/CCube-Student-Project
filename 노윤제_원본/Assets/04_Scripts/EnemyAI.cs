//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyAI : MonoBehaviour
//{
//    [Header("플레이어")]
//    public Transform player;

//    [Header("시야")]
//    public float viewDistance = 15f;
//    public float viewAngle = 60f;

//    [Header("속도")]
//    public float patrolSpeed = 3f;
//    public float chaseSpeed = 12f;

//    [Header("회전")]
//    public float rotationSpeed = 20f;

//    [Header("순찰 위치")]
//    public Transform[] patrolPoints;

//    [Header("애니메이션")]
//    public Animator animator;

//    [Header("랜덤 정지")]
//    [Range(0f, 1f)]
//    public float stopChance = 0.4f;     // 40% 확률
//    public float stopDuration = 2f;     // 멈추는 시간

//    private NavMeshAgent agent;
//    private int currentPoint;
//    private bool chasing;

//    private bool isStopped = false;
//    private float stopTimer = 0f;
//    private float nextStopCheck = 0f;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();

//        // 미끄러짐 감소
//        agent.acceleration = 100f;

//        // 방향 전환 속도 증가
//        agent.angularSpeed = 1500f;

//        // 직접 회전 처리
//        agent.updateRotation = false;

//        // 도착 판정 거리
//        agent.stoppingDistance = 0.1f;

//        agent.speed = patrolSpeed;

//        if (patrolPoints.Length > 0)
//        {
//            agent.SetDestination(patrolPoints[0].position);
//        }

//        // 처음 랜덤 검사 시간
//        nextStopCheck = Time.time + Random.Range(3f, 7f);
//    }

//    void Update()
//    {
//        // ---------------------------
//        // 랜덤 정지
//        // ---------------------------
//        if (isStopped)
//        {
//            agent.isStopped = true;

//            if (animator != null)
//            {
//                animator.SetBool("Idle", true);
//                animator.SetBool("Run", false);
//            }

//            stopTimer -= Time.deltaTime;

//            if (stopTimer <= 0f)
//            {
//                isStopped = false;
//                agent.isStopped = false;
//                nextStopCheck = Time.time + Random.Range(3f, 7f);
//            }

//            return;
//        }

//        // 일정 시간마다 랜덤으로 멈춤
//        if (Time.time >= nextStopCheck)
//        {
//            nextStopCheck = Time.time + Random.Range(3f, 7f);

//            if (Random.value < stopChance)
//            {
//                isStopped = true;
//                stopTimer = stopDuration;
//                return;
//            }
//        }

//        // ---------------------------
//        // 이동 애니메이션
//        // ---------------------------
//        if (animator != null)
//        {
//            //bool moving = agent.velocity.magnitude > 0.1f;

//            //animator.SetBool("Run", moving);
//            //animator.SetBool("Idle", !moving);

//            bool moving =
//            !agent.isStopped &&
//            agent.hasPath &&
//            agent.remainingDistance > agent.stoppingDistance;

//            animator.SetBool("Run", moving);
//            animator.SetBool("Idle", !moving);
//        }

//        // ---------------------------
//        // 이동 방향 바라보기
//        // ---------------------------
//        if (agent.velocity.sqrMagnitude > 0.1f)
//        {
//            Quaternion targetRotation =
//                Quaternion.LookRotation(agent.velocity.normalized);

//            transform.rotation = Quaternion.Slerp(
//                transform.rotation,
//                targetRotation,
//                rotationSpeed * Time.deltaTime);
//        }

//        // ---------------------------
//        // 플레이어 발견
//        // ---------------------------
//        if (CanSeePlayer())
//        {
//            chasing = true;
//        }

//        // ---------------------------
//        // AI 행동
//        // ---------------------------
//        if (chasing)
//        {
//            ChasePlayer();
//        }
//        else
//        {
//            Patrol();
//        }
//    }

//    bool CanSeePlayer()
//    {
//        if (player == null)
//            return false;

//        Vector3 dirToPlayer = player.position - transform.position;

//        if (dirToPlayer.magnitude > viewDistance)
//            return false;

//        float angle =
//            Vector3.Angle(transform.forward, dirToPlayer);

//        if (angle > viewAngle * 0.5f)
//            return false;

//        RaycastHit hit;

//        if (Physics.Raycast(
//            transform.position + Vector3.up,
//            dirToPlayer.normalized,
//            out hit,
//            viewDistance))
//        {
//            Debug.Log(hit.collider.name);

//            if (hit.transform == player)
//            {
//                return true;
//            }
//        }

//        return false;
//    }

//    void ChasePlayer()
//    {
//        agent.speed = chaseSpeed;
//        agent.SetDestination(player.position);
//    }

//    void Patrol()
//    {
//        if (patrolPoints.Length == 0)
//            return;

//        agent.speed = patrolSpeed;

//        if (!agent.pathPending &&
//            agent.remainingDistance <= agent.stoppingDistance)
//        {
//            currentPoint++;

//            if (currentPoint >= patrolPoints.Length)
//                currentPoint = 0;

//            agent.SetDestination(
//                patrolPoints[currentPoint].position);
//        }
//    }

//#if UNITY_EDITOR
//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.yellow;

//        Gizmos.DrawWireSphere(transform.position, viewDistance);

//        Vector3 leftDir =
//            Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward;

//        Vector3 rightDir =
//            Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward;

//        Gizmos.color = Color.red;
//        Gizmos.DrawLine(transform.position,
//            transform.position + leftDir * viewDistance);

//        Gizmos.DrawLine(transform.position,
//            transform.position + rightDir * viewDistance);

//        Gizmos.color = Color.blue;
//        Gizmos.DrawLine(transform.position,
//            transform.position + transform.forward * viewDistance);
//    }
//#endif
//}
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("플레이어")]
    public Transform player;

    [Header("시야")]
    public float viewDistance = 15f;
    public float viewAngle = 60f;

    [Header("속도")]
    public float patrolSpeed = 3f;
    public float chaseSpeed = 12f;

    [Header("회전")]
    public float rotationSpeed = 20f;

    [Header("순찰 위치")]
    public Transform[] patrolPoints;

    [Header("애니메이션")]
    public Animator animator;

    [Header("랜덤 정지")]
    [Range(0f, 1f)]
    public float stopChance = 0.4f;
    public float stopDuration = 2f;

    private NavMeshAgent agent;
    private int currentPoint;
    private bool chasing;

    private bool isStopped = false;
    private float stopTimer = 0f;
    private float nextStopCheck = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 미끄러짐 감소
        agent.acceleration = 100f;

        // 방향 전환 속도 증가
        agent.angularSpeed = 1500f;

        // 직접 회전 처리
        agent.updateRotation = false;

        // 도착 판정 거리
        agent.stoppingDistance = 0.1f;

        agent.speed = patrolSpeed;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position); //처음가는 곳을 설정
        }

        
        nextStopCheck = Time.time + Random.Range(3f, 7f);  //멈추는 시간 랜덤으로 설정
    }

    void Update()
    {
        // ---------------------------
        // 랜덤 정지
        // ---------------------------
        if (isStopped)
        {
            agent.isStopped = true;

            if (animator != null)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Run", false);
            }

            stopTimer -= Time.deltaTime;

            if (stopTimer <= 0f)
            {
                isStopped = false;
                agent.isStopped = false;

                nextStopCheck =
                    Time.time + Random.Range(3f, 7f);
            }

            return;
        }

        // ---------------------------
        // 일정 시간마다 랜덤으로 멈춤
        // ---------------------------
        if (Time.time >= nextStopCheck)
        {
            nextStopCheck =
                Time.time + Random.Range(3f, 7f);

            if (Random.value < stopChance)
            {
                isStopped = true;
                stopTimer = stopDuration;

                return;
            }
        }

        // ---------------------------
        // 이동 애니메이션
        // ---------------------------
        if (animator != null)
        {
            bool moving =
                !agent.isStopped &&
                agent.hasPath &&
                agent.remainingDistance > agent.stoppingDistance;

            animator.SetBool("Run", moving);
            animator.SetBool("Idle", !moving);
        }

        // ---------------------------
        // 이동 방향 바라보기
        // ---------------------------
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    agent.velocity.normalized // 방향값을 얻기위해 벡터 정규화
                );

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime // 어디서부터 어디까지 얼마만큼씩 회전할지 정한다
                );
        }

        // ---------------------------
        // 플레이어 발견
        // ---------------------------
        if (CanSeePlayer())
        {
            // 처음 발견했을 때만 실행
            if (!chasing)
            {
                chasing = true;

                // 생존 시간 시작
                SurvivalTime survivalTime =
                    player.GetComponent<SurvivalTime>();

                if (survivalTime != null)
                {
                    survivalTime.StartChase(); // 생존시간이 없을때 추적 시작
                }
            }
        }

        // ---------------------------
        // AI 행동
        // ---------------------------
        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;
        
        //플레이어까지의 거리
        Vector3 dirToPlayer = player.position - transform.position; 

        // 시야 거리
        if (dirToPlayer.magnitude > viewDistance) //플레이어까지의 거리가 시야거리보다 클때 끄기
            return false;

        // 괴물과 플레이어사이의 각도
        float angle =
            Vector3.Angle(transform.forward,dirToPlayer);//괴물이 보는 방향부터 플레이어방향까지의 각도

        if (angle > viewAngle * 0.5f) return false;               

        RaycastHit hit;

        // 구조물에 막혀 있는지 확인 
        if (Physics.Raycast(transform.position + Vector3.up,dirToPlayer.normalized,out hit,viewDistance))
        {
            Debug.Log(hit.collider.name);

            if (hit.transform == player) return true; // 플레이어가 시야각안에 있다고 판단   
        }
        return false;
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed; //괴물의 속도를 추적속도로 변경

        agent.SetDestination(player.position); //괴물의 이동방햑을 플레이어로 고정
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.speed = patrolSpeed; //괴물의 속도를 순찰 속도로 고정

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance) // 경로가 정해지고 남은거리가 멈추는 거리 이하일때 
        {
            currentPoint++;                                    // 패트럴 포인트의 인덱스 1추가

            if (currentPoint >= patrolPoints.Length)           // 패트럴 포인트 배열의 길이보다 현재 포인트 변수값이 더 크면 0으로 초기화
            {
                currentPoint = 0;
            }

            agent.SetDestination(
                patrolPoints[currentPoint].position            // Ai이동 목적지를 패트럴포인트 배열의 현재 인덱스 위치로 설정
            );
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            viewDistance
        );

        Vector3 leftDir =
            Quaternion.Euler(
                0,
                -viewAngle * 0.5f,
                0
            ) * transform.forward;

        Vector3 rightDir =
            Quaternion.Euler(
                0,
                viewAngle * 0.5f,
                0
            ) * transform.forward;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            transform.position,
            transform.position +
            leftDir * viewDistance
        );

        Gizmos.DrawLine(
            transform.position,
            transform.position +
            rightDir * viewDistance
        );

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(
            transform.position,
            transform.position +
            transform.forward * viewDistance
        );
    }
#endif
}