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
    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        Time.timeScale = 1;

        PoolingManager.Instance.CreatePool("Enemy", enemyPrefab, 20);


        selectedClassInt = PlayerPrefs.GetInt("SelectedCharacter");
        selectedClass = characterClasses[selectedClassInt];

        PlayerInfos.Instance.characterClass = selectedClass;
        EquipSelectedAbilities();
    }

    void Start()
    {
        //PoolingManager.Instance.CreatePool("Enemy", enemyPrefab, 20);
        //PoolingManager.Instance.CreatePool("Projectile", projectilePrefab, 20);
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
