using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public static WayPointManager instance;

    public Transform[] waypoints;

    void Awake()
    {
        instance = this;
    }
}