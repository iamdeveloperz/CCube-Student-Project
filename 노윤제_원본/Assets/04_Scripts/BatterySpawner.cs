using UnityEngine;
using System.Collections;

public class BatterySpawner : MonoBehaviour
{
    [Header("배터리 프리팹")]
    public GameObject batteryPrefab;

    [Header("스폰 위치")]
    public Transform[] spawnPoints;

    [Header("스폰 시간")]
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 10f;

    [Header("동시에 존재할 수 있는 배터리")]
    public int maxBatteries = 1;

    private int currentBatteries = 0;

    void Start()
    {
        StartCoroutine(SpawnBattery());
    }

    IEnumerator SpawnBattery()
    {
        while (true)
        {
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            yield return new WaitForSeconds(spawnTime);

            if (currentBatteries < maxBatteries)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        if (batteryPrefab == null)
        {
            Debug.LogError("Battery Prefab이 지정되지 않았습니다.");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn Point가 없습니다.");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);

        GameObject battery = Instantiate(
            batteryPrefab,
            spawnPoints[randomIndex].position,
            spawnPoints[randomIndex].rotation
        );

        currentBatteries++;

        StartCoroutine(CheckBatteryDestroyed(battery));
    }

    IEnumerator CheckBatteryDestroyed(GameObject battery)
    {
        while (battery != null)
        {
            yield return null;
        }

        currentBatteries--;
    }
}