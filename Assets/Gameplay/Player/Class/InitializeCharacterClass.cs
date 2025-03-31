using System.Collections.Generic;
using UnityEngine;

public class InitializeCharacterClass : MonoBehaviour
{
    [SerializeField] private CharacterClass[] characterClasses;

    [SerializeField] private UpgradeableAbility[] movementAbilities;
    [SerializeField] private UpgradeableAbility[] attackAbilities;

    public CharacterClass selectedClass;
    private int selectedClassInt;

    //pools
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject xpCapsulePrefab;

    private void Awake()
    {
        Time.timeScale = 1;
        selectedClassInt = PlayerPrefs.GetInt("SelectedCharacter");
        selectedClass = characterClasses[selectedClassInt];

        PlayerInfos.Instance.characterClass = selectedClass;
        EquipSelectedAbilities();

        PoolingManager.Instance.CreatePool("Enemy(Clone)", enemyPrefab, 20);
        PoolingManager.Instance.CreatePool("XP Capsule(Clone)", xpCapsulePrefab, 10);
    }

    private void EquipSelectedAbilities()
    {
        int movementIndex = PlayerPrefs.GetInt("MovementAbility");
        if (movementIndex >= 0 && movementIndex < selectedClass.upgradeableAbilities.Length)
        {
            selectedClass.upgradeableAbilities[0] = movementAbilities[movementIndex];
        }

        for (int i = 0; i < 3; i++)
        {
            int abilityIndex = PlayerPrefs.GetInt($"Ability_{i}");
            if (abilityIndex >= 0 && abilityIndex < selectedClass.upgradeableAbilities.Length)
            {
                selectedClass.upgradeableAbilities[i + 1] = attackAbilities[abilityIndex];
            }
        }
    }
}
