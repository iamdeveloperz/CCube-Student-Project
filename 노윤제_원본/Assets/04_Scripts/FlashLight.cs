////using UnityEngine;

////public class Flashlight : MonoBehaviour
////{
////    public Light flashlight;

////    void Update()
////    {
////        if (Input.GetKeyDown(KeyCode.F))
////        {
////            flashlight.enabled = !flashlight.enabled;
////        }
////    }
////}

//using UnityEngine;
//using UnityEngine.UI;

//public class Flashlight : MonoBehaviour
//{
//    [Header("손전등")]
//    public Light flashlight;

//    [Header("배터리")]
//    public float battery = 100f;
//    public Slider batterySlider;

//    private float timer;

//    void Start()
//    {

//    }

//    void Update()
//    {
//        // F키로 손전등 ON/OFF
//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            // 배터리가 남아있을 때만 켜짐
//            if (battery > 0)
//            {
//                flashlight.enabled = !flashlight.enabled;
//            }
//        }

//        // 손전등이 켜져있으면 배터리 감소
//        if (flashlight.enabled)
//        {
//            timer += Time.deltaTime;

//            if (timer >= 1f)
//            {
//                timer = 0f;

//                battery--;

//                if (battery < 0)
//                    battery = 0;

//                if (batterySlider != null)
//                    batterySlider.value = battery;

//                // 배터리 다 떨어짐
//                if (battery <= 0)
//                {
//                    flashlight.enabled = false;
//                }
//            }
//        }
//    }

//    // ===========================
//    // 배터리 충전 함수
//    // ===========================
//    public void AddBattery(float amount)
//    {
//        battery += amount;

//        if (battery > 100)
//            battery = 100;

//        if (batterySlider != null)
//            batterySlider.value = battery;
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("손전등")]
    public Light flashlight;

    [Header("배터리")]
    public float maxBattery = 100f;
    public float battery = 100f;
    public float drainPerSecond = 1f;

    [Header("배터리 UI")]
    public Slider batterySlider;

    void Start()
    {
        battery = maxBattery;

        if (batterySlider != null)
        {
            batterySlider.maxValue = maxBattery;
            batterySlider.value = battery;
        }
    }

    void Update()
    {
        // F키로 손전등 켜기/끄기
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (battery > 0f)
            {
                flashlight.enabled = !flashlight.enabled;
            }
        }

        // 손전등 사용 중 배터리 감소
        if (flashlight.enabled)
        {
            battery -= drainPerSecond * Time.deltaTime;
            battery = Mathf.Clamp(battery, 0f, maxBattery);

            // 배터리 UI 업데이트
            UpdateBatteryUI();

            // 배터리 0% → 손전등 강제 OFF
            if (battery <= 0f)
            {
                flashlight.enabled = false;
            }
        }
        else
        {
            // 손전등이 꺼져 있어도 UI는 최신 상태 유지
            UpdateBatteryUI();
        }
    }

    // 배터리 아이템 획득
    public void AddBattery(float amount)
    {
        battery += amount;
        battery = Mathf.Clamp(battery, 0f, maxBattery);

        UpdateBatteryUI();

        Debug.Log("배터리 +" + amount + "% / 현재 배터리: " + battery + "%");
    }

    // 배터리 UI 업데이트
    void UpdateBatteryUI()
    {
        if (batterySlider != null)
        {
            batterySlider.value = battery;
        }
    }
}