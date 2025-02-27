using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public List<UpgradeableAbility> abilitiesForSale;
    public Transform abilityListParent;
    public GameObject abilityButtonPrefab;

    private void Start()
    {
        PopulateStore();
    }

    private void PopulateStore()
    {
        foreach (var ability in abilitiesForSale)
        {
            GameObject buttonObj = Instantiate(abilityButtonPrefab, abilityListParent);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            int currentLevel = PlayerPrefs.GetInt(ability.name, 0);
            buttonText.text = $"{ability.abilityLevels[0].abilityName} - Niveau {currentLevel}";

            button.onClick.AddListener(() => UpgradeAbility(ability, buttonText));
        }
    }

    private void UpgradeAbility(UpgradeableAbility ability, Text buttonText)
    {
        int currentLevel = PlayerPrefs.GetInt(ability.name, 0);
        if (currentLevel < ability.abilityLevels.Count - 1)
        {
            currentLevel++;
            PlayerPrefs.SetInt(ability.name, currentLevel);
            PlayerPrefs.Save();
            buttonText.text = $"{ability.abilityLevels[0].abilityName} - Niveau {currentLevel}";
            Debug.Log($"Amélioration : {ability.abilityLevels[0].abilityName} Niveau {currentLevel}");
        }
        else
        {
            Debug.Log("Niveau max atteint !");
        }
    }
}
