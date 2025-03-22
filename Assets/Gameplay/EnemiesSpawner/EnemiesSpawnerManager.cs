using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesSpawnerManager : MonoBehaviour
{
    public List<WaveConfig> waves;
    private int currentWaveIndex = 0;
    private bool isWaveActive = false;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject victoryMenu;

    private int activeEnemiesCount;

    void Start()
    {
        if (waves.Count > 0)
        {
            StartCoroutine(SpawnWaves());
        }
        else
        {
            Debug.LogError("Aucune vague définie dans la liste des vagues.");
        }
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            WaveConfig currentWave = waves[currentWaveIndex];
            Debug.Log($"Démarrage de la vague {currentWaveIndex + 1}/{waves.Count}");

            isWaveActive = true;

            float waveTimer = 0f;
            while (waveTimer < currentWave.waveDuration)
            {
                SpawnEnemyFromWave(currentWave);

                waveTimer += currentWave.spawnRate;
                yield return new WaitForSeconds(currentWave.spawnRate);
            }

            Debug.Log($"Vague {currentWaveIndex + 1} terminée.");
            isWaveActive = false;
            currentWaveIndex++;

            if (currentWaveIndex >= waves.Count)
            {
                StartCoroutine(WaitForEnemiesToDie());
            }
        }
    }

    void SpawnEnemyFromWave(WaveConfig wave)
    {
        float totalChance = 0f;

        foreach (var enemy in wave.enemies)
        {
            totalChance += enemy.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var enemy in wave.enemies)
        {
            cumulativeChance += enemy.spawnChance;

            if (randomValue <= cumulativeChance)
            {
                EnemiesInfos selectedEnemyInfo = enemy.enemyInfo;
                Vector2 spawnPosition = GetRandomSpawnPosition();

                GameObject enemyInstance = PoolingManager.Instance.GetFromPool("Enemy", spawnPosition, Quaternion.identity);
                //GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                EnemyInfo enemyInfoScript = enemyInstance.GetComponent<EnemyInfo>();
                if (enemyInfoScript != null)
                {
                    enemyInfoScript.ApplyEnemyInfo(selectedEnemyInfo);
                }

                break;
            }
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        Camera mainCamera = Camera.main;
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float screenHeight = mainCamera.orthographicSize;

        // Choisir une position en dehors de l'écran, à gauche/droite ou en haut/bas
        float xPosition = Random.Range(-screenWidth - 5f, screenWidth + 5f);  // Hors de l'écran horizontalement
        float yPosition = Random.Range(-screenHeight - 5f, screenHeight + 5f);  // Hors de l'écran verticalement

        return new Vector2(xPosition, yPosition);
    }

    private IEnumerator WaitForEnemiesToDie()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        activeEnemiesCount = allEnemies.Length;

        LifeManager.OnDeath += () => activeEnemiesCount--;

        while (activeEnemiesCount > 0)
        {
            yield return null;
        }

        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5f);

        Time.timeScale = 0f;
        victoryMenu.SetActive(true);
    }
}
