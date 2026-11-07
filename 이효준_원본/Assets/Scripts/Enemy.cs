
//using UnityEngine;
//using UnityEngine.AI;

//public class Enemy : MonoBehaviour
//{
//    // 인스펙터창에서 설정 가능
//    [Header("플레이어")]
//    public Transform player;

//    [Header("이동")]
//    public float patrolRadius = 20f;
//    public float detectDistance = 30f;
//    public float viewAngle = 100f;

//    public float patrolSpeed = 4f;
//    public float chaseSpeed = 9f;

//    [Header("추격 설정")]
//    public float searchTime = 4f;
//    public float searchRadius = 8f;

//    // 플레이어를 놓친 뒤 수색으로 바뀌기까지 기다리는 시간
//    public float lostPlayerDelay = 5f;

//    [Header("추격 사운드")]
//    public AudioSource chaseAudio;

//    private NavMeshAgent agent;
//    private Vector3 startPosition;

//    // 마지막으로 플레이어를 본 위치
//    private Vector3 lastSeenPosition;

//    // 탐색 시작 시간
//    private float searchTimer;

//    // 플레이어를 놓친 후 지난 시간
//    private float lostPlayerTimer;

//    // 3개의 상태
//    // Patrol = 순찰
//    // Chase = 추격
//    // Search = 탐색
//    enum State
//    {
//        Patrol,
//        chase,
//        Search
//    }

//    State state = State.Patrol;


//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();

//        startPosition = transform.position;

//        agent.speed = patrolSpeed;
//        agent.isStopped = false;

//        // 추격 사운드가 자동으로 시작하지 않게 함
//        if (chaseAudio != null)
//        {
//            chaseAudio.loop = true;
//            chaseAudio.playOnAwake = false;
//        }

//        GoToRandomPoint();
//    }


//    void Update()
//    {
//        if (player == null)
//            return;

//        bool canSeePlayer = CanSeePlayer();
//        // 플레이어를 발견하면 추격
//        if (canSeePlayer)
//        {
//            // 플레이어를 다시 발견했으므로
//            // 놓친 시간 초기화
//            lostPlayerTimer = 0f;
//            // 마지막으로 본 위치 저장
//            lastSeenPosition = player.position;
//            if (state != State.chase)
//            {
//                StartChase();
//            }
//            state = State.chase;
//        }
//        // 상태별 행동
//        switch (state)
//        {
//            case State.Patrol: //맵을 돌아다니면서 탐색하는 일반적인 상태
//                Patrol();
//                break;
//            case State.chase: //플레이어를 발견하면 플레이어를 추격하는 상태
//                Chase(canSeePlayer);
//                break;
//            case State.Search: //플레이어를 추격하다 놓치면 플레이어에 마지막 위치까지 가서 주변을 탐색하는 상태
//                Search(canSeePlayer);
//                break;
//        }
//    }


//    // =========================================
//    // 순찰
//    // =========================================

//    void Patrol()
//    {
//        agent.isStopped = false;
//        agent.speed = patrolSpeed;

//        // 순찰 중에는 추격 사운드 정지
//        StopChaseSound();

//        if (!agent.pathPending &&
//            agent.remainingDistance < 0.5f)
//        {
//            GoToRandomPoint();
//        }
//    }


//    // =========================================
//    // 추격
//    // =========================================

//    void Chase(bool canSeePlayer)
//    {
//        agent.isStopped = false;
//        agent.speed = chaseSpeed;

//        // 추격 중에는 사운드 재생
//        PlayChaseSound();


//        // -----------------------------------------
//        // 플레이어가 보이는 경우
//        // -----------------------------------------

//        if (canSeePlayer)
//        {
//            // 플레이어를 보고 있으므로
//            // 놓친 시간을 0으로 초기화
//            lostPlayerTimer = 0f;

//            // 플레이어의 현재 위치를 계속 저장
//            lastSeenPosition = player.position;

//            // 플레이어를 계속 추격
//            agent.SetDestination(player.position);
//        }


//        // -----------------------------------------
//        // 플레이어가 보이지 않는 경우
//        // -----------------------------------------

//        else
//        {
//            // 플레이어를 놓친 시간 계산
//            lostPlayerTimer += Time.deltaTime;

//            // 마지막으로 플레이어를 본 위치까지 이동
//            agent.SetDestination(lastSeenPosition);


//            // 플레이어를 놓친 상태가
//            // 5초 이상 지속되면 수색 모드로 변경
//            if (lostPlayerTimer >= lostPlayerDelay)
//            {
//                StartSearch();
//            }
//        }
//    }


//    // =========================================
//    // 탐색
//    // =========================================

//    void Search(bool canSeePlayer)
//    {
//        // 다시 플레이어를 보면
//        // 즉시 추격
//        if (canSeePlayer)
//        {
//            state = State.chase;

//            searchTimer = 0f;
//            lostPlayerTimer = 0f;

//            PlayChaseSound();

//            return;
//        }

//        // 탐색 중에는 추격 사운드 정지
//        StopChaseSound();

//        agent.isStopped = false;
//        agent.speed = chaseSpeed;

//        searchTimer += Time.deltaTime;


//        // 탐색 시간이 끝나면 순찰
//        if (searchTimer >= searchTime)
//        {
//            state = State.Patrol;

//            agent.speed = patrolSpeed;

//            StopChaseSound();

//            GoToRandomPoint();

//            return;
//        }


//        // 목적지에 도착하면
//        // 새로운 탐색 위치로 이동
//        if (!agent.pathPending &&
//            agent.remainingDistance < 0.5f)
//        {
//            GoToSearchPoint();
//        }
//    }


//    // =========================================
//    // 추격 시작
//    // =========================================

//    void StartChase()
//    {
//        state = State.chase;

//        // 추격 시작 시
//        // 플레이어를 놓친 시간 초기화
//        lostPlayerTimer = 0f;

//        agent.isStopped = false;
//        agent.speed = chaseSpeed;

//        // 추격 사운드 시작
//        PlayChaseSound();
//    }


//    // =========================================
//    // 탐색 시작
//    // =========================================

//    void StartSearch()
//    {
//        state = State.Search;

//        // 탐색 시간 초기화
//        searchTimer = 0f;

//        agent.speed = chaseSpeed;

//        // 추격 사운드 정지
//        StopChaseSound();

//        // 마지막으로 본 위치 주변 탐색
//        GoToSearchPoint();
//    }


//    // =========================================
//    // 추격 사운드 재생
//    // =========================================

//    void PlayChaseSound()
//    {
//        if (chaseAudio != null && !chaseAudio.isPlaying)
//        {
//            chaseAudio.Play();
//        }
//    }


//    // =========================================
//    // 추격 사운드 정지
//    // =========================================

//    void StopChaseSound()
//    {
//        if (chaseAudio != null && chaseAudio.isPlaying)
//        {
//            chaseAudio.Stop();
//        }
//    }


//    // =========================================
//    // 랜덤 순찰 위치
//    // =========================================

//    void GoToRandomPoint()
//    {
//        Vector3 randomDirection =
//            Random.insideUnitSphere * patrolRadius;

//        randomDirection += startPosition;

//        NavMeshHit hit;

//        if (NavMesh.SamplePosition(
//            randomDirection,
//            out hit,
//            patrolRadius,
//            NavMesh.AllAreas))
//        {
//            agent.isStopped = false;
//            agent.SetDestination(hit.position);
//        }
//    }


//    // =========================================
//    // 탐색 위치
//    // =========================================

//    void GoToSearchPoint()
//    {
//        Vector3 randomDirection =
//            Random.insideUnitSphere * searchRadius;

//        randomDirection += lastSeenPosition;

//        NavMeshHit hit;

//        if (NavMesh.SamplePosition(
//            randomDirection,
//            out hit,
//            searchRadius,
//            NavMesh.AllAreas))
//        {
//            agent.isStopped = false;
//            agent.SetDestination(hit.position);
//        }
//    }


//    // =========================================
//    // 플레이어 시야 확인
//    // =========================================

//    bool CanSeePlayer()
//    {
//        // Enemy의 눈 위치
//        Vector3 eye = transform.position + Vector3.up * 1.6f;
//        // 플레이어를 바라볼 위치
//        Vector3 targetPlayer = player.position + Vector3.up * 1.0f;
//        // Enemy에서 플레이어까지의 거리
//        Vector3 distanceOrigin = targetPlayer - eye;

//        float distance = distanceOrigin.magnitude;

//        // 감지 거리 확인
//        if (distance > detectDistance) return false;
//        // 정규화를 통한 방향값으로 전환
//        distanceOrigin.Normalize();

//        // 시야각 확인
//        float angle = Vector3.Angle(transform.forward,distanceOrigin);

//        if (angle > viewAngle / 2f) return false;

//        // Raycast로 벽이나 장애물 확인
//        if (Physics.Raycast(
//            eye,
//            distanceOrigin,
//            out RaycastHit hit,
//            distance))
//        {
//            // Raycast가 플레이어를 맞췄다면
//            // 플레이어를 볼 수 있음
//            if (hit.transform == player ||
//                hit.transform.root == player.root)
//            {
//                return true;
//            }

//            // 벽이나 다른 장애물에 막혔다면
//            // 플레이어를 볼 수 없음
//            return false;
//        }

//        return false;
//    }
//}

//using UnityEngine;
//using UnityEngine.AI;

//public class Enemy : MonoBehaviour
//{

//    //인스펙터창에서 설정 가능
//    [Header("플레이어")]
//    public Transform player;

//    [Header("이동")]
//    public float patrolRadius = 20f;
//    public float detectDistance = 30f;
//    public float viewAngle = 100f;

//    public float patrolSpeed = 4f;
//    public float chaseSpeed = 9f;

//    [Header("추격 설정")]
//    public float searchTime = 4f;
//    public float searchRadius = 8f;

//    [Header("추격 사운드")]
//    public AudioSource chaseAudio;

//    private NavMeshAgent agent;
//    private Vector3 startPosition;

//    // 마지막으로 플레이어를 본 위치
//    private Vector3 lastSeenPosition;

//    // 탐색 시작 시간
//    private float searchTimer;

//    // 3개의 상태 페트롤, 추격, 탐색
//    enum State
//    {
//        Patrol,
//        Chase,
//        Search
//    }

//    State state = State.Patrol;


//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();

//        startPosition = transform.position;

//        agent.speed = patrolSpeed;
//        agent.isStopped = false;

//        // 추격 사운드가 자동으로 시작하지 않게 함
//        if (chaseAudio != null)
//        {
//            chaseAudio.loop = true;
//            chaseAudio.playOnAwake = false;
//        }

//        GoToRandomPoint();
//    }


//    void Update()
//    {
//        if (player == null)
//            return;

//        bool canSeePlayer = CanSeePlayer();


//        // =========================================
//        // 플레이어를 발견하면 항상 추격
//        // =========================================

//        if (canSeePlayer)
//        {
//            lastSeenPosition = player.position;

//            if (state != State.Chase)
//            {
//                StartChase();
//            }

//            state = State.Chase;
//        }


//        // =========================================
//        // 상태별 행동
//        // =========================================

//        switch (state)
//        {
//            case State.Patrol:
//                Patrol();
//                break;

//            case State.Chase:
//                Chase(canSeePlayer);
//                break;

//            case State.Search:
//                Search(canSeePlayer);
//                break;
//        }
//    }


//    // =========================================
//    // 순찰
//    // =========================================

//    void Patrol()
//    {
//        agent.isStopped = false;
//        agent.speed = patrolSpeed;

//        // 순찰 중에는 추격 사운드 정지
//        StopChaseSound();

//        if (!agent.pathPending &&
//            agent.remainingDistance < 0.5f)
//        {
//            GoToRandomPoint();
//        }
//    }


//    // =========================================
//    // 추격
//    // =========================================

//    void Chase(bool canSeePlayer)
//    {
//        agent.isStopped = false;
//        agent.speed = chaseSpeed;

//        // 추격 중에는 사운드 재생
//        PlayChaseSound();

//        // 플레이어가 보이는 동안에는
//        // 계속 현재 위치를 추적
//        if (canSeePlayer)
//        {
//            lastSeenPosition = player.position;

//            agent.SetDestination(player.position);
//        }
//        else
//        {
//            // 플레이어가 안 보이면
//            // 마지막으로 본 위치로 이동
//            agent.SetDestination(lastSeenPosition);

//            // 마지막 위치에 도착
//            if (!agent.pathPending &&
//                agent.remainingDistance < 1f)
//            {
//                StartSearch();
//            }
//        }
//    }


//    // =========================================
//    // 탐색
//    // =========================================

//    void Search(bool canSeePlayer)
//    {
//        // 다시 플레이어를 보면
//        // 즉시 추격
//        if (canSeePlayer)
//        {
//            state = State.Chase;
//            searchTimer = 0f;

//            PlayChaseSound();

//            return;
//        }

//        // 탐색 중에는 추격 사운드 정지
//        StopChaseSound();

//        agent.isStopped = false;
//        agent.speed = chaseSpeed;

//        searchTimer += Time.deltaTime;


//        // 탐색 시간이 끝나면 순찰
//        if (searchTimer >= searchTime)
//        {
//            state = State.Patrol;

//            agent.speed = patrolSpeed;

//            StopChaseSound();

//            GoToRandomPoint();

//            return;
//        }


//        // 목적지에 도착하면
//        // 새로운 탐색 위치로 이동
//        if (!agent.pathPending &&
//            agent.remainingDistance < 0.5f)
//        {
//            GoToSearchPoint();
//        }
//    }


//    // =========================================
//    // 추격 시작
//    // =========================================

//    void StartChase()
//    {
//        state = State.Chase;

//        agent.isStopped = false;
//        agent.speed = chaseSpeed;

//        // 추격 사운드 시작
//        PlayChaseSound();
//    }


//    // =========================================
//    // 탐색 시작
//    // =========================================

//    void StartSearch()
//    {
//        state = State.Search;

//        searchTimer = 0f;

//        agent.speed = chaseSpeed;

//        // 추격 사운드 정지
//        StopChaseSound();

//        GoToSearchPoint();
//    }


//    // =========================================
//    // 추격 사운드 재생
//    // =========================================

//    void PlayChaseSound()
//    {
//        if (chaseAudio != null && !chaseAudio.isPlaying)
//        {
//            chaseAudio.Play();
//        }
//    }


//    // =========================================
//    // 추격 사운드 정지
//    // =========================================

//    void StopChaseSound()
//    {
//        if (chaseAudio != null && chaseAudio.isPlaying)
//        {
//            chaseAudio.Stop();
//        }
//    }


//    // =========================================
//    // 랜덤 순찰 위치
//    // =========================================

//    void GoToRandomPoint()
//    {
//        Vector3 randomDirection =
//            Random.insideUnitSphere * patrolRadius;

//        randomDirection += startPosition;

//        NavMeshHit hit;

//        if (NavMesh.SamplePosition(
//            randomDirection,
//            out hit,
//            patrolRadius,
//            NavMesh.AllAreas))
//        {
//            agent.isStopped = false;
//            agent.SetDestination(hit.position);
//        }   
//    }


//    // =========================================
//    // 탐색 위치
//    // =========================================

//    void GoToSearchPoint()
//    {
//        Vector3 randomDirection =
//            Random.insideUnitSphere * searchRadius;

//        randomDirection += lastSeenPosition;

//        NavMeshHit hit;

//        if (NavMesh.SamplePosition(
//            randomDirection,
//            out hit,
//            searchRadius,
//            NavMesh.AllAreas))
//        {
//            agent.isStopped = false;
//            agent.SetDestination(hit.position);
//        }
//    }


//    // =========================================
//    // 플레이어 시야 확인
//    // =========================================

//    bool CanSeePlayer()
//    {
//        Vector3 eye =
//            transform.position + Vector3.up * 1.6f;

//        Vector3 target =
//            player.position + Vector3.up * 1.0f;

//        Vector3 direction =
//            target - eye;

//        float distance =
//            direction.magnitude;


//        // 거리
//        if (distance > detectDistance)
//            return false;

//        direction.Normalize();


//        // 시야각
//        float angle =
//            Vector3.Angle(
//                transform.forward,
//                direction
//            );

//        if (angle > viewAngle / 2f)
//            return false;


//        // 벽 / 장애물
//        if (Physics.Raycast(
//            eye,
//            direction,
//            out RaycastHit hit,
//            distance))
//        {
//            if (hit.transform == player ||
//                hit.transform.root == player.root)
//            {
//                return true;
//            }

//            return false;
//        }

//        return false;
//    }
//}

using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // 인스펙터창에서 설정 가능
    [Header("플레이어")]
    public Transform player;

    [Header("이동")]
    public float patrolRadius = 20f;
    public float detectDistance = 30f;
    public float viewAngle = 100f;

    public float patrolSpeed = 4f;
    public float chaseSpeed = 9f;

    [Header("추격 설정")]
    public float searchTime = 4f;
    public float searchRadius = 8f;

    // 플레이어를 놓친 뒤 수색으로 바뀌기까지 기다리는 시간
    public float lostPlayerDelay = 5f;

    [Header("추격 사운드")]
    public AudioSource chaseAudio;

    // 발소리 설정 (추가)
    [Header("발소리")]
    public AudioSource footstepAudio;
    public AudioClip footstepClip;
    public float patrolFootstepInterval = 0.55f;
    public float chaseFootstepInterval = 0.3f;
    public float searchFootstepInterval = 0.5f;
    public float footstepMaxDistance = 15f;

    // 다음 발소리까지 남은 시간
    private float footstepTimer;

    private NavMeshAgent agent;
    private Vector3 startPosition;

    // 마지막으로 플레이어를 본 위치
    private Vector3 lastSeenPosition;

    // 탐색 시작 시간
    private float searchTimer;

    // 플레이어를 놓친 후 지난 시간
    private float lostPlayerTimer;

    // 3개의 상태
    // Patrol = 순찰
    // Chase = 추격
    // Search = 탐색
    enum State
    {
        Patrol,
        chase,
        Search
    }

    State state = State.Patrol;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        startPosition = transform.position;

        agent.speed = patrolSpeed;
        agent.isStopped = false;

        // 추격 사운드가 자동으로 시작하지 않게 함
        if (chaseAudio != null)
        {
            chaseAudio.loop = true;
            chaseAudio.playOnAwake = false;
        }

        // 발소리 설정 (추가)
        if (footstepAudio != null)
        {
            footstepAudio.playOnAwake = false;
            footstepAudio.loop = false;

            // 발소리를 3D 사운드로 설정해 거리에 따라 작아지게 함
            footstepAudio.spatialBlend = 1f;
            footstepAudio.rolloffMode = AudioRolloffMode.Logarithmic;
            footstepAudio.minDistance = 1f;
            footstepAudio.maxDistance = footstepMaxDistance;
        }

        GoToRandomPoint();
    }


    void Update()
    {
        if (player == null)
            return;

        bool canSeePlayer = CanSeePlayer();
        // 플레이어를 발견하면 추격
        if (canSeePlayer)
        {
            // 플레이어를 다시 발견했으므로
            // 놓친 시간 초기화
            lostPlayerTimer = 0f;
            // 마지막으로 본 위치 저장
            lastSeenPosition = player.position;
            if (state != State.chase)
            {
                StartChase();
            }
            state = State.chase;
        }
        // 상태별 행동
        switch (state)
        {
            case State.Patrol: //맵을 돌아다니면서 탐색하는 일반적인 상태
                Patrol();
                break;
            case State.chase: //플레이어를 발견하면 플레이어를 추격하는 상태
                Chase(canSeePlayer);
                break;
            case State.Search: //플레이어를 추격하다 놓치면 플레이어에 마지막 위치까지 가서 주변을 탐색하는 상태
                Search(canSeePlayer);
                break;
        }

        // 발소리 처리 (추가)
        UpdateFootsteps();
    }


    // =========================================
    // 순찰
    // =========================================

    void Patrol()
    {
        agent.isStopped = false;
        agent.speed = patrolSpeed;

        // 순찰 중에는 추격 사운드 정지
        StopChaseSound();

        if (!agent.pathPending &&
            agent.remainingDistance < 0.5f)
        {
            GoToRandomPoint();
        }
    }


    // =========================================
    // 추격
    // =========================================

    void Chase(bool canSeePlayer)
    {
        agent.isStopped = false;
        agent.speed = chaseSpeed;

        // 추격 중에는 사운드 재생
        PlayChaseSound();


        // -----------------------------------------
        // 플레이어가 보이는 경우
        // -----------------------------------------

        if (canSeePlayer)
        {
            // 플레이어를 보고 있으므로
            // 놓친 시간을 0으로 초기화
            lostPlayerTimer = 0f;

            // 플레이어의 현재 위치를 계속 저장
            lastSeenPosition = player.position;

            // 플레이어를 계속 추격
            agent.SetDestination(player.position);
        }


        // -----------------------------------------
        // 플레이어가 보이지 않는 경우
        // -----------------------------------------

        else
        {
            // 플레이어를 놓친 시간 계산
            lostPlayerTimer += Time.deltaTime;

            // 마지막으로 플레이어를 본 위치까지 이동
            agent.SetDestination(lastSeenPosition);


            // 플레이어를 놓친 상태가
            // 5초 이상 지속되면 수색 모드로 변경
            if (lostPlayerTimer >= lostPlayerDelay)
            {
                StartSearch();
            }
        }
    }


    // =========================================
    // 탐색
    // =========================================

    void Search(bool canSeePlayer)
    {
        // 다시 플레이어를 보면
        // 즉시 추격
        if (canSeePlayer)
        {
            state = State.chase;

            searchTimer = 0f;
            lostPlayerTimer = 0f;

            PlayChaseSound();

            return;
        }

        // 탐색 중에는 추격 사운드 정지
        StopChaseSound();

        agent.isStopped = false;
        agent.speed = chaseSpeed;

        searchTimer += Time.deltaTime;


        // 탐색 시간이 끝나면 순찰
        if (searchTimer >= searchTime)
        {
            state = State.Patrol;

            agent.speed = patrolSpeed;

            StopChaseSound();

            GoToRandomPoint();

            return;
        }


        // 목적지에 도착하면
        // 새로운 탐색 위치로 이동
        if (!agent.pathPending &&
            agent.remainingDistance < 0.5f)
        {
            GoToSearchPoint();
        }
    }


    // =========================================
    // 추격 시작
    // =========================================

    void StartChase()
    {
        state = State.chase;

        // 추격 시작 시
        // 플레이어를 놓친 시간 초기화
        lostPlayerTimer = 0f;

        agent.isStopped = false;
        agent.speed = chaseSpeed;

        // 추격 사운드 시작
        PlayChaseSound();
    }


    // =========================================
    // 탐색 시작
    // =========================================

    void StartSearch()
    {
        state = State.Search;

        // 탐색 시간 초기화
        searchTimer = 0f;

        agent.speed = chaseSpeed;

        // 추격 사운드 정지
        StopChaseSound();

        // 마지막으로 본 위치 주변 탐색
        GoToSearchPoint();
    }


    // =========================================
    // 추격 사운드 재생
    // =========================================

    void PlayChaseSound()
    {
        if (chaseAudio != null && !chaseAudio.isPlaying)
        {
            chaseAudio.Play();
        }
    }


    // =========================================
    // 추격 사운드 정지
    // =========================================

    void StopChaseSound()
    {
        if (chaseAudio != null && chaseAudio.isPlaying)
        {
            chaseAudio.Stop();
        }
    }


    // =========================================
    // 랜덤 순찰 위치
    // =========================================

    void GoToRandomPoint()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * patrolRadius;

        randomDirection += startPosition;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomDirection,
            out hit,
            patrolRadius,
            NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }


    // =========================================
    // 탐색 위치
    // =========================================

    void GoToSearchPoint()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * searchRadius;

        randomDirection += lastSeenPosition;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            randomDirection,
            out hit,
            searchRadius,
            NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
    }


    // =========================================
    // 플레이어 시야 확인
    // =========================================

    bool CanSeePlayer()
    {
        // Enemy의 눈 위치
        Vector3 eye = transform.position + Vector3.up * 1.6f;
        // 플레이어를 바라볼 위치
        Vector3 targetPlayer = player.position + Vector3.up * 1.0f;
        // Enemy에서 플레이어까지의 거리
        Vector3 distanceOrigin = targetPlayer - eye;

        float distance = distanceOrigin.magnitude;

        // 감지 거리 확인
        if (distance > detectDistance) return false;
        // 정규화를 통한 방향값으로 전환
        distanceOrigin.Normalize();

        // 시야각 확인
        float angle = Vector3.Angle(transform.forward, distanceOrigin);

        if (angle > viewAngle / 2f) return false;

        // Raycast로 벽이나 장애물 확인
        if (Physics.Raycast(
            eye,
            distanceOrigin,
            out RaycastHit hit,
            distance))
        {
            // Raycast가 플레이어를 맞췄다면
            // 플레이어를 볼 수 있음
            if (hit.transform == player ||
                hit.transform.root == player.root)
            {
                return true;
            }

            // 벽이나 다른 장애물에 막혔다면
            // 플레이어를 볼 수 없음
            return false;
        }

        return false;
    }


    // =========================================
    // 발소리 처리 (추가)
    // =========================================

    void UpdateFootsteps()
    {
        // 발소리 AudioSource 또는 오디오 파일이 없으면 실행하지 않음
        if (footstepAudio == null || footstepClip == null)
            return;

        // 몬스터가 정지했거나 실제로 움직이지 않으면 발소리를 재생하지 않음
        if (agent.isStopped || agent.velocity.sqrMagnitude < 0.1f)
        {
            footstepTimer = 0f;
            return;
        }

        // 현재 상태에 따라 발소리 간격 설정
        float interval = patrolFootstepInterval;

        switch (state)
        {
            case State.Patrol:
                interval = patrolFootstepInterval;
                break;

            case State.chase:
                interval = chaseFootstepInterval;
                break;

            case State.Search:
                interval = searchFootstepInterval;
                break;
        }

        // 발소리 재생 간격을 계산
        footstepTimer += Time.deltaTime;

        // 설정한 시간이 지나면 발소리 한 번 재생
        if (footstepTimer >= interval)
        {
            footstepAudio.PlayOneShot(footstepClip);
            footstepTimer = 0f;
        }
    }
}