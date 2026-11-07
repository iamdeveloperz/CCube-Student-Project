using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    private Animator _animator;

    [Header("이동")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 1.5f;
    public float gravity = -9.81f;

    [Header("웅크리기")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = 1f;
    public float crouchCameraHeight = 0.5f;
    public float crouchTransitionSpeed = 8f;

    [Header("마우스 시점")]
    public float mouseSensitivity = 200f;
    public Transform cameraTransform;

    [Header("발소리")]
    public AudioSource footstepSource;
    public AudioClip walkFootstep;
    public AudioClip runFootstep;

    // 걷기 발소리 간격
    public float walkStepInterval = 0.4f;

    // 달리기 발소리 간격
    public float runStepInterval = 0.22f;

    private float footstepTimer;


    private CharacterController controller;

    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isCrouching;

    private float xRotation = 0f;

    // 원래 CharacterController 값
    private float normalHeight;
    private float normalCenterY;

    // 원래 카메라 위치 설정
    private Vector3 normalCameraPosition;
    [Header("죽음")]
    public float deatY = -10f;
    public GameObject deathText;
    public float respawnDelay = 3.5f;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private bool isDead = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        // 현재 기본값 저장
        normalHeight = controller.height;
        normalCenterY = controller.center.y;

        normalCameraPosition = cameraTransform.localPosition;

        // 마우스 커서 숨기기 및 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (isDead)
            return;

        Look();
        Move();
        Crouch();
        Footsteps();

        if (transform.position.y < deatY)
        {
            Die();
        }
    }



    // 마우스 시점
 

    void Look()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity *
            Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity *
            Time.deltaTime;


        // 위아래 회전
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(
            xRotation,
            -90f,
            90f
        );


        cameraTransform.localRotation =
            Quaternion.Euler(
                xRotation,
                0f,
                0f
            );


        // 좌우 회전
        transform.Rotate(
            Vector3.up * mouseX
        );
    }


    // 이동
    
    void Move()
    {
        // 바닥 체크
        isGrounded = controller.isGrounded;


        // 바닥에 있을 때 중력값 유지
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }


        // 입력
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        // 이동 속도 결정
        float currentSpeed;

        if (isCrouching)
        {
            // 웅크리기
            currentSpeed = crouchSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            // 달리기
            currentSpeed = runSpeed;
        }
        else
        {
            // 걷기
            currentSpeed = speed;
        }


        // 이동 방향
        Vector3 moveMovement =
            (
                transform.right * x +
                transform.forward * z
            ).normalized * currentSpeed;


        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // 웅크리고 있을 때는 점프 불가능
            if (!isCrouching)
            {
                playerVelocity.y =
                    Mathf.Sqrt(
                        jumpForce * -2f * gravity
                    );

                Debug.Log(Mathf.Sqrt(jumpForce * -2f * gravity));
            }
        }


        // 중력
        playerVelocity.y +=
            gravity * Time.deltaTime;


        // 최종 이동
        Vector3 finalMotion = moveMovement;

        finalMotion.y = playerVelocity.y;


        controller.Move(
            finalMotion * Time.deltaTime
        );
    }

    // 발소리


    void Footsteps()
    {
        // 현재 입력값
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 실제로 움직이고 있는지 확인
        bool isMoving =
            Mathf.Abs(x) > 0.1f ||
            Mathf.Abs(z) > 0.1f;

        _animator.SetBool("IsWalk", isMoving);

        // 땅에 있고 움직이고 있을 때만 발소리
        if (isGrounded && isMoving)
        {
            // Shift를 누르고 있고 웅크리지 않았다면 달리기
            bool isRunning =
                Input.GetKey(KeyCode.LeftShift) &&
                !isCrouching;

            // 발소리 간격 결정
            float stepInterval =
                isRunning
                    ? runStepInterval
                    : walkStepInterval;

            // 타이머 감소
            footstepTimer -= Time.deltaTime;

            // 발소리 재생
            if (footstepTimer <= 0f)
            {
                if (footstepSource != null)
                {
                    if (isRunning && runFootstep != null)
                    {
                        footstepSource.PlayOneShot(runFootstep);
                    }
                    else if (!isRunning && walkFootstep != null)
                    {
                        footstepSource.PlayOneShot(walkFootstep);
                    }
                }

                // 다음 발소리까지 기다리는 시간
                footstepTimer = stepInterval;
            }
        }
        else
        {
            // 멈추면 다음 발소리를 다시 준비
            footstepTimer = 0f;
        }
    }


    // =====================================================
    // 웅크리기
    // =====================================================

    void Crouch()
    {
        // Ctrl을 누르고 있는 동안 웅크리기
        isCrouching = Input.GetKey(crouchKey);


        // ---------------------------------------------
        // 목표 높이
        // ---------------------------------------------

        float targetHeight =
            isCrouching
                ? crouchHeight
                : normalHeight;


        // 높이를 부드럽게 변경
        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            crouchTransitionSpeed * Time.deltaTime
        );


        // ---------------------------------------------
        // CharacterController 중심
        // ---------------------------------------------

        float targetCenterY =
            isCrouching
                ? normalCenterY -
                  (normalHeight - crouchHeight) / 2f
                : normalCenterY;


        Vector3 center = controller.center;


        center.y = Mathf.Lerp(
            center.y,
            targetCenterY,
            crouchTransitionSpeed * Time.deltaTime
        );


        controller.center = center;


        // ---------------------------------------------
        // 카메라 높이
        // ---------------------------------------------

        float targetCameraY =
            isCrouching
                ? crouchCameraHeight
                : normalCameraPosition.y;


        Vector3 cameraPosition =
            cameraTransform.localPosition;


        cameraPosition.y = Mathf.Lerp(
            cameraPosition.y,
            targetCameraY,
            crouchTransitionSpeed * Time.deltaTime
        );


        cameraTransform.localPosition =
            cameraPosition;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (deathText != null)
        {
            deathText.SetActive(true);
        }

        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        controller.enabled = false;

        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        playerVelocity = Vector3.zero;
        xRotation = 0f;

        controller.enabled = true;

        if (deathText != null)
        {
            deathText.SetActive(false);
        }

        isDead = false;
    }

    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log("Hit Player");
        if (isDead) return;

        if (hit.gameObject.CompareTag("Monster"))
        {
            Die();
        }
    }
}