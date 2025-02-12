using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Scriptable Objects/Enemies")]
public class EnemiesInfos : ScriptableObject
{
    public Sprite sprite;
    public string enemyName;
    public float maxLife;
    public int damages;
}
