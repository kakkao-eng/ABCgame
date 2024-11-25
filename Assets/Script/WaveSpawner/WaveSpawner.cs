using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // Prefab ของมอนสเตอร์
    public Transform spawnPoint;      // ตำแหน่งที่มอนสเตอร์จะเกิด
    public int enemiesPerWave = 5;    // จำนวนมอนสเตอร์ต่อเวฟ
    public float spawnInterval = 1f;  // ระยะเวลาระหว่างการเกิดของแต่ละมอนสเตอร์
    public float waveInterval = 5f;   // ระยะเวลาระหว่างแต่ละเวฟ

    private int waveIndex = 1;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            Debug.Log($"Starting Wave {waveIndex}");

            // วนลูปสร้างมอนสเตอร์ตามจำนวน enemiesPerWave
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            // รอ waveInterval วินาทีเพื่อเริ่มเวฟถัดไป
            yield return new WaitForSeconds(waveInterval);

            waveIndex++;
            enemiesPerWave += 2; // เพิ่มจำนวนมอนสเตอร์ในเวฟถัดไปเพื่อความยากที่เพิ่มขึ้น
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}