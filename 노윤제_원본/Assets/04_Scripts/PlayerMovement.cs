using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 속도")] 
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("가속")]
    public float acceleration = 3f;
    public float maxAccelerationSpeed = 12f;

    [Header("마우스 감도")]
    public float mouseSensitivity = 200f;

    [Header("카메라")]
    public Transform playerCamera;

    private Rigidbody rb;
    private float xRotation = 0f;

    // 현재 가속된 속도
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 리지드 바디 컴포넌트 가져오기.

        currentSpeed = walkSpeed; // 현재속도는 걷는 속도.

        Cursor.lockState = CursorLockMode.Locked; //마우스 고정하기.
        Cursor.visible = false; // 마우스 커서 안보이게
    }

    void Update()
    {
        // 마우스 이동
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; //마우스 좌우 움직임 입력
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; //마우스 위아래 움직임 입력

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMovingForward = Input.GetKey(KeyCode.W);

        // 기본 속도
        float baseSpeed = isRunning ? runSpeed : walkSpeed;

        // W를 누르고 있을 때 가속
        if (isMovingForward)
        {
            //현재 속도에서 프레임에따라 정해진값만큼 곱해서 더한다
            currentSpeed += acceleration * Time.fixedDeltaTime;

            // 현재속도를 처음속도와 최고속도 사이로  제한
            currentSpeed = Mathf.Clamp(currentSpeed,baseSpeed,maxAccelerationSpeed); 
        }
        else
        {   
            // W를 떼면 기본 속도로 즉시 초기화
            currentSpeed = baseSpeed;
        }

        // x,y,z 이동속도 일정하게 보정
        Vector3 move = (transform.forward * v + transform.right * h).normalized; 
        
        //움직이는 속도
        rb.velocity = new Vector3( move.x * currentSpeed,rb.velocity.y,move.z * currentSpeed);
    }
}