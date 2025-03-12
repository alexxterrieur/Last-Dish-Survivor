using UnityEngine;

public class InitializeCharacterClass : MonoBehaviour
{
    [SerializeField] private CharacterClass[] characterClasses;
    public CharacterClass selectedClass;
    private int selectedClassInt;

    private void Awake()
    {
        selectedClassInt = PlayerPrefs.GetInt("SelectedCharacter", 0);
        selectedClass = characterClasses[selectedClassInt];

        PlayerInfos.Instance.characterClass = selectedClass;
        EquipSelectedAbilities();
    }

    private void EquipSelectedAbilities()
    {
        int movementIndex = PlayerPrefs.GetInt("MovementAbility", -1);
        if (movementIndex >= 0 && movementIndex < selectedClass.upgradeableAbilities.Length)
        {
            PlayerInfos.Instance.AddAbility(selectedClass.upgradeableAbilities[movementIndex]);
        }

        for (int i = 0; i < 3; i++)
        {
            int abilityIndex = PlayerPrefs.GetInt($"Ability_{i + 1}", -1);
            if (abilityIndex >= 0 && abilityIndex < selectedClass.upgradeableAbilities.Length)
            {
                PlayerInfos.Instance.AddAbility(selectedClass.upgradeableAbilities[abilityIndex]);
            }
        }
    }
}
