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

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                object choice = choices[i];
                string choiceName = choice is UpgradeableWeapon ? ((UpgradeableWeapon)choice).weaponLevels[0].abilityName : ((UpgradeableBonus)choice).bonusLevels[0].bonusName;

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

        // Vérification des armes disponibles non encore équipées ou pas au max
        List<UpgradeableWeapon> availableWeapons = allWeapons
            .Where(weapon => !player.weaponLevels.ContainsKey(weapon) || player.weaponLevels[weapon] < weapon.weaponLevels.Count - 1)
            .ToList();

        // Vérification des bonus disponibles non encore équipés ou pas au max
        List<UpgradeableBonus> availableBonuses = allBonuses
            .Where(bonus => !player.bonusLevels.ContainsKey(bonus) || player.bonusLevels[bonus] < bonus.bonusLevels.Count - 1)
            .ToList();

        // Ajoute des armes et bonus si la limite n'est pas atteinte
        if (player.weaponLevels.Count < player.maxWeapons && availableWeapons.Count > 0)
        {
            possibleChoices.Add(availableWeapons[Random.Range(0, availableWeapons.Count)]);
        }
        if (player.bonusLevels.Count < player.maxBonuses && availableBonuses.Count > 0)
        {
            possibleChoices.Add(availableBonuses[Random.Range(0, availableBonuses.Count)]);
        }

        // Si la limite d'arme ou bonus est atteinte, proposer uniquement des objets existants à améliorer
        if (player.weaponLevels.Count >= player.maxWeapons || player.bonusLevels.Count >= player.maxBonuses)
        {
            // Filtrer les armes ou bonus déjà possédés et améliorables (pas au niveau max)
            List<object> upgradeOptions = new List<object>();

            // Filtrer les armes déjà possédées et pas au max
            upgradeOptions.AddRange(player.weaponLevels.Keys.Where(weapon => player.weaponLevels[weapon] < weapon.weaponLevels.Count - 1));
            // Filtrer les bonus déjà possédés et pas au max
            upgradeOptions.AddRange(player.bonusLevels.Keys.Where(bonus => player.bonusLevels[bonus] < bonus.bonusLevels.Count - 1));

            // Si aucune option d'amélioration n'est disponible (toutes au max), on ne propose rien
            if (upgradeOptions.Count == 0)
            {
                Debug.Log("Toutes les armes et bonus sont au niveau maximum.");
                return possibleChoices;  // Aucun autre choix n'est disponible
            }

            // Mélanger les options et choisir 3 parmi elles
            possibleChoices.AddRange(upgradeOptions.OrderBy(x => Random.value).Take(3));
        }

        // Si nous avons trop de choix, limiter à 3
        return possibleChoices.OrderBy(x => Random.value).Take(3).ToList();
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
