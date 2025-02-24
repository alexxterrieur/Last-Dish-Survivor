using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public static PlayerInfos Instance { get; private set; }

    public CharacterClass characterClass;
    SpriteRenderer spriteRenderer;

    private float maxHealth;
    private float speed;
    private float damageBonus;

    public int maxWeapons = 4;
    public int maxBonuses = 4;

    public Dictionary<UpgradeableWeapon, int> weaponLevels = new Dictionary<UpgradeableWeapon, int>();
    public Dictionary<UpgradeableBonus, int> bonusLevels = new Dictionary<UpgradeableBonus, int>();

    public List<Ability> activeAbilities = new List<Ability>();

    LifeManager lifeManager;
    AbilityManager abilityManager;
    [SerializeField] private WeaponsBonusUI weaponsBonusUI;

    PlayerMovement playerMovement;

    public enum PlayerDirection { Right, Left, Up, Down }
    public PlayerDirection currentDirection = PlayerDirection.Right;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (characterClass != null)
        {
            InitializeStats();
            EquipStartingWeapons();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterClass.playerSprite;

        lifeManager = GetComponent<LifeManager>();
        abilityManager = GetComponent<AbilityManager>();
        playerMovement = GetComponent<PlayerMovement>();

        if (lifeManager != null)
        {
            lifeManager.Initialize(maxHealth);
        }

        if (abilityManager != null)
        {
            foreach (var weaponEntry in weaponLevels)
            {
                abilityManager.AddWeapon(weaponEntry.Key, weaponEntry.Value);
            }
        }
    }

    public void UpdatePlayerDirection(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
            {
                currentDirection = moveDirection.x > 0 ? PlayerDirection.Right : PlayerDirection.Left;
            }
            else
            {
                currentDirection = moveDirection.y > 0 ? PlayerDirection.Up : PlayerDirection.Down;
            }
        }
    }

    private void InitializeStats()
    {
        speed = characterClass.speed;
        maxHealth = characterClass.maxHealth;
        damageBonus = characterClass.damageBonus;

        activeAbilities.AddRange(characterClass.activeAbilities);
    }

    private void EquipStartingWeapons()
    {
        foreach (var weapon in characterClass.startWeapon)
        {
            AddWeapon(weapon);
        }
    }

    public void AddAbility(Ability newAbility)
    {
        if (!activeAbilities.Contains(newAbility))
        {
            activeAbilities.Add(newAbility);
            Debug.Log($"Nouvelle capacité apprise : {newAbility.abilityName}");
        }
    }

    public void AddWeapon(UpgradeableWeapon upgradeableWeapon)
    {
        if (weaponLevels.ContainsKey(upgradeableWeapon))
        {
            //Upgrade weapon
            weaponLevels[upgradeableWeapon]++;
            Debug.Log($"Amélioration de {upgradeableWeapon.weaponLevels[0].abilityName} au niveau {weaponLevels[upgradeableWeapon] + 1} !");
        }
        else if (weaponLevels.Count < maxWeapons)
        {
            //Add weapon with index 0 of upgradeabloWeapon
            weaponLevels[upgradeableWeapon] = 0;
            Debug.Log($"Nouvelle arme équipée : {upgradeableWeapon.weaponLevels[0].abilityName}");

            abilityManager?.AddWeapon(upgradeableWeapon, 0);
        }
        else
        {
            Debug.Log("Nombre maximum d'armes atteint !");
        }

        UpdateUI();
    }

    public void AddBonus(UpgradeableBonus upgradeableBonus)
    {
        if (bonusLevels.ContainsKey(upgradeableBonus))
        {
            //Upgrade Bonus
            bonusLevels[upgradeableBonus]++;
            Debug.Log($"Amélioration du bonus {upgradeableBonus.bonusLevels[0].bonusName} au niveau {bonusLevels[upgradeableBonus] + 1} !");
        }
        else if (bonusLevels.Count < maxBonuses)
        {
            //Add weapon with index 0 of upgradeabloBonus
            bonusLevels[upgradeableBonus] = 0;
            Debug.Log($"Nouveau bonus acquis : {upgradeableBonus.bonusLevels[0].bonusName}");
        }
        else
        {
            Debug.Log("Nombre maximum de bonus atteint !");
            return;
        }

        Bonus currentBonus = upgradeableBonus.GetBonusAtLevel(bonusLevels[upgradeableBonus]);
        currentBonus.ApplyEffect(this);

        UpdateUI();
    }

    private void UpdateUI()
    {
        weaponsBonusUI?.UpdateUI(weaponLevels, bonusLevels);
    }

    public void IncreaseMaxHealth(float amount) => lifeManager.maxHealth += amount;
    public void IncreaseCurrentHealth(float amount)
    {
        lifeManager.currentHealth += amount;

        if (lifeManager.currentHealth > lifeManager.maxHealth)
        {
            lifeManager.currentHealth = lifeManager.maxHealth;
        }
    }

    public void IncreaseDamageBonus(float amount) => damageBonus += amount;
    public void IncreaseSpeed(float amount) => speed += amount;

    public float GetMaxHealth() => maxHealth;
    public float GetSpeed() => speed;
    public float GetDamageBonus() => damageBonus;
}


[System.Serializable]
public class DropItem
{
    public GameObject itemPrefab;
    [Range(0f, 1f)] public float dropChance;
}
