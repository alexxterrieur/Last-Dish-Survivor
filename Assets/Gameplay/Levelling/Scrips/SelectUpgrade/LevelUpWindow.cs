using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] choiceButtons;
    public List<UpgradeableWeapon> allWeapons;
    public List<UpgradeableBonus> allBonuses;

    private void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void OpenLevelUpMenu()
    {
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);

        List<object> choices = GetRandomChoices();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Count && choices[i] != null)
            {
                object choice = choices[i];

                //Get button childs
                Transform buttonTransform = choiceButtons[i].transform;
                Image iconImage = buttonTransform.GetChild(0).GetComponent<Image>();
                TMP_Text nameText = buttonTransform.GetChild(1).GetComponent<TMP_Text>();
                TMP_Text descriptionText = buttonTransform.GetChild(2).GetComponent<TMP_Text>();
                TMP_Text levelText = buttonTransform.GetChild(3).GetComponent<TMP_Text>();

                PlayerInfos player = PlayerInfos.Instance;

                if (choice is UpgradeableWeapon weapon)
                {
                    int nextLevel;
                    if (player.weaponLevels.ContainsKey(weapon))
                    {
                        nextLevel = player.weaponLevels[weapon] + 1; //Next level
                    }
                    else
                    {
                        nextLevel = 0; //New weapon
                    }

                    //Get next level infos
                    Weapon weaponData = weapon.GetWeaponAtLevel(nextLevel);
                    nameText.text = weaponData.abilityName;
                    descriptionText.text = weaponData.description;
                    iconImage.sprite = weaponData.icon;
                    levelText.text = player.weaponLevels.ContainsKey(weapon) ? "Lv. " + (nextLevel + 1) : "New !";
                }
                else if (choice is UpgradeableBonus bonus)
                {
                    int nextLevel;
                    if (player.bonusLevels.ContainsKey(bonus))
                    {
                        nextLevel = player.bonusLevels[bonus] + 1; //Next level
                    }
                    else
                    {
                        nextLevel = 0; //New bonus
                    }

                    //Get next level infos
                    Bonus bonusData = bonus.GetBonusAtLevel(nextLevel);
                    nameText.text = bonusData.bonusName;
                    descriptionText.text = bonusData.description;
                    iconImage.sprite = bonusData.icon;
                    levelText.text = player.bonusLevels.ContainsKey(bonus) ? "Lv. " + (nextLevel + 1) : "New !";
                }

                //Enable button and add event
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => SelectOption(choice));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private List<object> GetRandomChoices()
    {
        PlayerInfos player = PlayerInfos.Instance;
        List<object> possibleChoices = new List<object>();

        List<UpgradeableWeapon> availableWeapons = allWeapons.Where(w => !player.weaponLevels.ContainsKey(w)).ToList();
        List<UpgradeableBonus> availableBonuses = allBonuses.Where(b => !player.bonusLevels.ContainsKey(b)).ToList();

        if (player.weaponLevels.Count < player.maxWeapons)
        {
            possibleChoices.AddRange(availableWeapons);
        }
        if (player.bonusLevels.Count < player.maxBonuses)
        {
            possibleChoices.AddRange(availableBonuses);
        }

        List<object> upgradeOptions = new List<object>();

        foreach (var weapon in player.weaponLevels.Keys)
        {
            if (player.weaponLevels[weapon] < weapon.weaponLevels.Count - 1)
            {
                upgradeOptions.Add(weapon);
            }
        }

        foreach (var bonus in player.bonusLevels.Keys)
        {
            if (player.bonusLevels[bonus] < bonus.bonusLevels.Count - 1)
            {
                upgradeOptions.Add(bonus);
            }
        }

        upgradeOptions = upgradeOptions.OrderBy(_ => Random.value).ToList();

        while (possibleChoices.Count < 3 && upgradeOptions.Count > 0)
        {
            possibleChoices.Add(upgradeOptions[0]);
            upgradeOptions.RemoveAt(0);
        }

        while (possibleChoices.Count < 3)
        {
            possibleChoices.Add(null);
        }

        return possibleChoices;
    }

    public void SelectOption(object choice)
    {
        PlayerInfos player = PlayerInfos.Instance;

        if (choice is UpgradeableWeapon weapon)
        {
            player.AddWeapon(weapon);
        }
        else if (choice is UpgradeableBonus bonus)
        {
            player.AddBonus(bonus);
        }

        CloseLevelUpMenu();
    }

    public void CloseLevelUpMenu()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
