//using UnityEngine;

//public class BatteryPickup : MonoBehaviour
//{
//    public float batteryAmount = 30f;
//    public AudioClip pickupSound;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            Flashlight flashlight = other.GetComponent<Flashlight>();

//            if (flashlight != null)
//            {
//                flashlight.AddBatteryAmount(batteryAmount);

//                if (pickupSound != null)
//                {
//                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
//                }

//                Destroy(gameObject);
//            }
//        }
//    }
//}

using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float batteryAmount = 30f;
    public AudioClip pickupSound;

    private bool playerInRange = false;
    private Flashlight flashlight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            flashlight = other.GetComponent<Flashlight>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            flashlight = null;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            if (flashlight != null)
            {
                flashlight.AddBatteryAmount(batteryAmount);

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(
                        pickupSound,
                        transform.position
                    );
                }

                Destroy(gameObject);
            }
        }
    }
}