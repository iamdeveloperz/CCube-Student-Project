using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;

    public float sensitivity = 200f;

    float xRotation = 0f;

    void Update()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            sensitivity *
            Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            sensitivity *
            Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation =
            Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseX);
    }
}