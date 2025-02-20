using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] choiceButtons;
    public List<Weapon> allWeapons;
    public List<Bonus> allBonuses;

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
            if (i < choices.Count)
            {
                object choice = choices[i];
                string choiceName = choice is Weapon ? ((Weapon)choice).abilityName : ((Bonus)choice).bonusName;

                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choiceName;
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
        List<object> possibleChoices = new List<object>();
        PlayerInfos player = PlayerInfos.Instance;

        // Si le joueur n'a pas atteint les limites, proposer armes et bonus aléatoires
        if (player.equippedWeapons.Count < player.maxWeapons || player.equippedBonuses.Count < player.maxBonuses)
        {
            List<Weapon> availableWeapons = allWeapons.Where(w => !player.equippedWeapons.Contains(w)).ToList();
            List<Bonus> availableBonuses = allBonuses.Where(b => !player.equippedBonuses.Contains(b)).ToList();

            if (availableWeapons.Count > 0) possibleChoices.Add(availableWeapons[Random.Range(0, availableWeapons.Count)]);
            if (availableBonuses.Count > 0) possibleChoices.Add(availableBonuses[Random.Range(0, availableBonuses.Count)]);
        }

        // Si le joueur a atteint les limites, proposer uniquement des améliorations d'objets existants
        if (possibleChoices.Count < 3)
        {
            if (player.equippedWeapons.Count > 0) possibleChoices.Add(player.equippedWeapons[Random.Range(0, player.equippedWeapons.Count)]);
            if (player.equippedBonuses.Count > 0) possibleChoices.Add(player.equippedBonuses[Random.Range(0, player.equippedBonuses.Count)]);
        }

        //Shuffle and chose options
        return possibleChoices.OrderBy(x => Random.value).Take(3).ToList();
    }

    public void SelectOption(object choice)
    {
        PlayerInfos player = PlayerInfos.Instance;

        if (choice is Weapon weapon)
        {
            player.AddWeapon(weapon);
        }
        else if (choice is Bonus bonus)
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