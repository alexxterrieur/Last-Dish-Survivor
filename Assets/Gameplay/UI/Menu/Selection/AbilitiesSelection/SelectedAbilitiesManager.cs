using UnityEngine;
using UnityEngine.UI;

public class SelectedAbilitiesManager : MonoBehaviour
{
    [SerializeField] private UpgradeableAbility selectedAbilityMovement;
    [SerializeField] private UpgradeableAbility[] selectedAbilities = new UpgradeableAbility[3];

    [SerializeField] private Image movementabilityIcon;
    [SerializeField] private Image[] abilitiesIcon;

    [SerializeField] private GameObject confirmButton;

    //public int ability1Index;
    //public int ability2Index;
    //public int ability3Index;

    public void SelectAbilityMovement(UpgradeableAbility ability)
    {
        selectedAbilityMovement = ability;
        UpdateUI();
    }

    public void SelectAbility(UpgradeableAbility ability)
    {
        for (int i = 0; i < selectedAbilities.Length; i++)
        {
            if (selectedAbilities[i] == null)
            {
                selectedAbilities[i] = ability;
                UpdateUI();
                return;
            }
        }
    }

    private void UpdateUI()
    {
        if (selectedAbilityMovement != null)
            movementabilityIcon.sprite = selectedAbilityMovement.abilityLevels[0].icon;

        for (int i = 0; i < abilitiesIcon.Length; i++)
        {
            if (selectedAbilities[i] != null)
                abilitiesIcon[i].sprite = selectedAbilities[i].abilityLevels[0].icon;
        }

        ShowConfirmButton();
    }

    //public void SaveSelectedAbility1(int abilityIndex)
    //{
    //    ability1Index = abilityIndex;
    //}
    //public void SaveSelectedAbility2(int abilityIndex)
    //{
    //    ability2Index = abilityIndex;
    //}
    //public void SaveSelectedAbility3(int abilityIndex)
    //{
    //    ability3Index = abilityIndex;
    //}

    public void SaveSelectedAbilities()
    {     
        PlayerPrefs.SetInt("Ability_0", selectedAbilities[0].abilityIndex);
        PlayerPrefs.SetInt("Ability_1", selectedAbilities[1].abilityIndex);
        PlayerPrefs.SetInt("Ability_2", selectedAbilities[2].abilityIndex);

        PlayerPrefs.Save();
    }


    public void SaveSelectedMovementAbility(int abilityIndex)
    {
        PlayerPrefs.SetInt("MovementAbility", abilityIndex);
        PlayerPrefs.Save();
    }

    private void ShowConfirmButton()
    {
        confirmButton.SetActive(AreAllAbilitiesSelected());
    }

    private bool AreAllAbilitiesSelected()
    {
        if (selectedAbilityMovement == null)
            return false;

        foreach (var ability in selectedAbilities)
        {
            if (ability == null)
                return false;
        }

        return true;
    }
}
