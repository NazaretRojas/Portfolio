using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<WaveData> waves;
    [SerializeField] private Waypoint waypointPath;

    [SerializeField] private float timeBetweenEnemies = 1f;
    [SerializeField] private float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private LevelManager levelManager;
    private bool allWavesSpawned = false;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        Enemy.OnEndReached += HandleEnemyRemoved;
        EnemyHealth.OnEnemyDied += HandleEnemyRemoved;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Count)
        {
            WaveData wave = waves[currentWave];
            Debug.Log($"Oleada {currentWave + 1} comenzando...");
            levelManager?.IncreaseWave();

            for (int i = 0; i < wave.enemyCount; i++)
            {
                SpawnRandomEnemyFromWave(wave);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }

            if (wave.bossPrefab != null)
            {
                yield return new WaitForSeconds(1f);
                SpawnEnemy(wave.bossPrefab);
            }

            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        while (GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length > 0)
        {
            yield return null;
        }

        Debug.Log("¡Todas las oleadas y enemigos han terminado!");


        levelManager?.LevelCompleted();
        allWavesSpawned = true;
        CheckLevelComplete();
    }

    private void SpawnRandomEnemyFromWave(WaveData wave)
    {
        if (wave.enemies == null || wave.enemies.Count == 0) return;

        GameObject prefab = wave.enemies[Random.Range(0, wave.enemies.Count)];
        SpawnEnemy(prefab);
    }

    private void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null) return;

        Vector3 spawnPosition = waypointPath.GetWaypointPosition(0);
        GameObject newEnemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetWaypoint(waypointPath);
        }

        enemiesAlive++;
    }

    private void HandleEnemyRemoved(Enemy enemy)
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (allWavesSpawned && enemiesAlive == 0)
        {
            levelManager?.LevelCompleted();
        }
    }

    private void OnDestroy()
    {
        Enemy.OnEndReached -= HandleEnemyRemoved;
        EnemyHealth.OnEnemyDied -= HandleEnemyRemoved;
    }
}
