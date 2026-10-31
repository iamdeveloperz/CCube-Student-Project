using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;

    public float spawnDelay = 1f;
    public float nextWaveDelay = 3f;

    public int wave = 1;
    public int zombiesPerWave = 10;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        var spawnPosition = WayPointManager.instance.waypoints[0].position;
        spawnPosition.y = 0f;
        while (true)
        {
            // 현재 웨이브 생성
            for (int i = 0; i < zombiesPerWave; i++)
            {
                Instantiate(
                    zombiePrefab,
                    spawnPosition,
                    Quaternion.identity);

                yield return new WaitForSeconds(spawnDelay);
            }

            // 살아있는 좀비가 모두 없어질 때까지 대기
            while (GameObject.FindGameObjectsWithTag("Zombie").Length > 0)
            {
                yield return null;
            }

            // 다음 웨이브까지 대기
            yield return new WaitForSeconds(nextWaveDelay);

            // 웨이브 증가
            wave++;
            zombiesPerWave += 5;

            WaveManager.instance.NextWave();
        }
    }
}