using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    public float currentXP;
    private float xpNeededForNextLevel;
    public int currentLevel;
    public LevelUpWindow levelUpManager;
    public TMP_Text levelText;
    public Slider xpBar;
    public float fillSpeed = 1000f;

    private float targetXP; 

    void Start()
    {
        currentLevel = 0;
        xpNeededForNextLevel = 15;
        xpBar.value = 0;
        targetXP = 0;

        levelText.text = "Level: " + (currentLevel + 1);
    }

    void Update()
    {
        xpBar.value = Mathf.Lerp(xpBar.value, targetXP, fillSpeed * Time.deltaTime);
    }

    public void AddXP(int xpValue)
    {
        currentXP += xpValue;
        if (currentXP >= xpNeededForNextLevel)
        {
            LevelUp();
        }
        targetXP = currentXP / xpNeededForNextLevel;
    }

    private void LevelUp()
    {
        currentLevel++;
        levelText.text = "Level: " + (currentLevel + 1);
        currentXP = 0;
        xpNeededForNextLevel *= 1.4f;
        levelUpManager.OpenLevelUpMenu();

        xpBar.value = 0;
        targetXP = 0;
    }
}
