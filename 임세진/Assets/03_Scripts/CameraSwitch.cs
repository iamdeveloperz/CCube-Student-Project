using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private static CameraSwitch _sInstance;
    public static CameraSwitch Instance => _sInstance;

    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;

    public PlayerMove playerMove;
    public Gun gun;

    public Camera FPSCam { get; private set; }
    public Camera TPSCam { get; private set; }

    public bool IsFPS => firstPersonCamera.gameObject.activeSelf;
    public bool IsTPS => thirdPersonCamera.gameObject.activeSelf;

    private void Awake()
    {
        _sInstance = this;

        FPSCam = firstPersonCamera.GetComponent<Camera>();
        TPSCam = thirdPersonCamera.GetComponent<Camera>();
    }

    void Start()
    {
        // 처음에는 FPS 모드
        firstPersonCamera.SetActive(true);
        thirdPersonCamera.SetActive(false);

        playerMove.enabled = true;
        gun.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            bool fps = firstPersonCamera.activeSelf;

            firstPersonCamera.SetActive(!fps);
            thirdPersonCamera.SetActive(fps);

            // FPS → TPS
            if (fps)
            {
                playerMove.enabled = false;
                gun.enabled = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            // TPS → FPS
            else
            {
                playerMove.enabled = true;
                gun.enabled = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}