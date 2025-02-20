using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public float currentXP;
    private float xpNeededForNextLevel;
    public int currentLevel;
    public LevelUpWindow levelUpManager;

    void Start()
    {
        currentLevel = 1;
        xpNeededForNextLevel = 15;
    }

    public void AddXP(int xpValue)
    {
        currentXP += xpValue;
        if (currentXP >= xpNeededForNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXP = 0;
        xpNeededForNextLevel *= 1.4f;

        levelUpManager.OpenLevelUpMenu();
    }
}
