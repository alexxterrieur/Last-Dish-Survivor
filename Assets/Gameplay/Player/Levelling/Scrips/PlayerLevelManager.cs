using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public float currentXP;
    private float xpNeededForNextLevel;
    public int currentLevel;

    void Start()
    {
        currentLevel = 1;
        xpNeededForNextLevel = 15;
    }


    public void AddXP(int xpValue)
    {
        currentXP += xpValue;

        if(currentXP >= xpNeededForNextLevel)
        {
            LevelUp();
        }
    }


    private void LevelUp()
    {
        //pause game
        //open choice panel
        currentLevel++;
        currentXP = 0;
        xpNeededForNextLevel = xpNeededForNextLevel * 1.4f;
        //calcul xp pour next level
    }


}
