using UnityEngine;

public class LowResCamera : MonoBehaviour
{
    public int width = 320;
    public int height = 180;

    void Start()
    {
        Screen.SetResolution(width, height, false);
    }
}