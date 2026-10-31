//using UnityEngine;

//public class ThirdPersonCamera : MonoBehaviour
//{
//    public Transform player;

//    public float mouseSensitivity = 200f;
//    public float distance = 4f;
//    public float height = 2f;

//    float xRotation = 20f;
//    float yRotation = 0f;

//    void Update()
//    {
//        yRotation += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
//        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

//        xRotation = Mathf.Clamp(xRotation, -30f, 70f);
//    }

//    void LateUpdate()
//    {
//        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

//        Vector3 offset = rotation * new Vector3(0, 0, -distance);

//        transform.position = player.position + Vector3.up * height + offset;

//        transform.LookAt(player.position + Vector3.up * height);
//    }
//}
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public float height = 15f;

    void LateUpdate()
    {
        transform.position = player.position + Vector3.up * height;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}