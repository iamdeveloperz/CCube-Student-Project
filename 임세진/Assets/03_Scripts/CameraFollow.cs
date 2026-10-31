using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // 이 체크를 끄면 카메라가 따라가지 않음
    public bool follow = true;

    void LateUpdate()
    {
        if (!follow)
            return;

        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}