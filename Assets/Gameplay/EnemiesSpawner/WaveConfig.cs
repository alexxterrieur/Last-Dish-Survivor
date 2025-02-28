using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Scriptable Objects/Waves/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public EnemiesInfos enemyInfo;
        [Range(0, 1)] public float spawnChance;
    }

    public EnemySpawnInfo[] enemies;
    [Range(0.1f, 2f)] public float spawnRate;
    public float waveDuration;
}
