using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip walkSound;
    public AudioClip runSound;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool moving = h != 0 || v != 0;
        bool running = Input.GetKey(KeyCode.LeftShift);

        if (moving)
        {
            AudioClip targetClip = running ? runSound : walkSound;

            if (audioSource.clip != targetClip)
            {
                audioSource.clip = targetClip;
                audioSource.Play();
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}