using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Scriptable Objects/Enemies")]
public class EnemiesInfos : ScriptableObject
{
    public Sprite sprite;
    public string enemyName;

    public float maxLife;
    public float attackRange;
    public float attackInterval;
    public float attackDamage;
    public float speed;

    public List<DropItem> dropItems;
}
