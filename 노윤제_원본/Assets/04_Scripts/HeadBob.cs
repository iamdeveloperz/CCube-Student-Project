using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("°È±â Èçµé¸²")]
    public float amount = 0.06f;
    public float speed = 10f;

    [Header("Á¡ÇÁ/ÂøÁö Èçµé¸²")]
    public float jumpOffset = 0.15f;
    public float landOffset = 0.25f;
    public float smoothSpeed = 12f;

    Vector3 startPos;
    float extraOffset = 0f;

    bool isGrounded = true;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 targetPos = startPos;

        // ¶¥ À§¿¡ ÀÖÀ» ¶§¸¸ °È±â Èçµé¸²
        if (isGrounded && (x != 0 || z != 0))
        {
            targetPos += Vector3.up *
                         Mathf.Sin(Time.time * speed) *
                         amount;
        }

        // Á¡ÇÁ/ÂøÁö Èçµé¸²
        extraOffset = Mathf.Lerp(extraOffset, 0f, smoothSpeed * Time.deltaTime);
        targetPos += Vector3.up * extraOffset;

        transform.localPosition = targetPos;
    }

    public void OnJump()
    {
        isGrounded = false;
        extraOffset = -jumpOffset;
    }

    public void OnLand()
    {
        isGrounded = true;
        extraOffset = -landOffset;
    }
}