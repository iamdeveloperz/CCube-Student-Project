using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Transform cam;

        if (firstPersonCamera.activeSelf)
            cam = firstPersonCamera.transform;
        else
            cam = thirdPersonCamera.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * v + right * h;

        transform.position += move * speed * Time.deltaTime;
    }
}