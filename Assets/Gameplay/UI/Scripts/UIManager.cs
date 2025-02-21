using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private int enemiesKilled;
    [SerializeField] TMP_Text enemiesKilledCount;

    private void Start()
    {
        enemiesKilledCount.text = " : 0";
        LifeManager.OnDeath += UpdateEnemiesKilled;
    }

    public void UpdateEnemiesKilled()
    {
        enemiesKilled++;
        enemiesKilledCount.text = " : " + enemiesKilled.ToString();
    }
}
