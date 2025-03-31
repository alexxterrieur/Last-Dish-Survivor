using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public static PlayerInfos Instance { get; private set; }

    public CharacterClass characterClass;
    SpriteRenderer spriteRenderer;

    private float maxHealth;
    private float speed;
    [SerializeField] private float damageBonus;

    public int maxWeapons = 4;
    public int maxBonuses = 4;

    public Dictionary<UpgradeableWeapon, int> weaponLevels = new Dictionary<UpgradeableWeapon, int>();
    public Dictionary<UpgradeableBonus, int> bonusLevels = new Dictionary<UpgradeableBonus, int>();
    public Dictionary<UpgradeableAbility, int> abilityLevels = new Dictionary<UpgradeableAbility, int>();

    public List<Ability> activeAbilities = new List<Ability>();

    LifeManager lifeManager;
    AbilityManager abilityManager;
    [SerializeField] private WeaponsBonusUI weaponsBonusUI;

    PlayerMovement playerMovement;
    public float xpBonus;

    public enum PlayerDirection { Right, Left, Up, Down }
    public PlayerDirection currentDirection = PlayerDirection.Right;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => characterClass != null);
        //yield return new WaitForSeconds(0.1f);

        LoadAbilityLevels();

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

            foreach (var abilityEntry in abilityLevels)
            {
                abilityManager.EquipAbility(abilityEntry.Key, abilityEntry.Value);
            }
        }

        UpdateUI();
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

        foreach (var upgradeableAbility in characterClass.upgradeableAbilities)
        {
            int abilityLevel = abilityLevels.ContainsKey(upgradeableAbility) ? abilityLevels[upgradeableAbility] : 0;
            Ability abilityInstance = upgradeableAbility.GetAbilityAtLevel(abilityLevel);
            if (abilityInstance != null)
            {
                activeAbilities.Add(abilityInstance);
            }
        }
    }

    private void EquipStartingWeapons()
    {
        foreach (var weapon in characterClass.startWeapon)
        {
            AddWeapon(weapon);
        }
    }

    public void AddWeapon(UpgradeableWeapon upgradeableWeapon)
    {
        if (weaponLevels.ContainsKey(upgradeableWeapon))
        {
            weaponLevels[upgradeableWeapon]++;
            Debug.Log($"Amélioration de {upgradeableWeapon.weaponLevels[0].abilityName} au niveau {weaponLevels[upgradeableWeapon] + 1} !");
        }
        else if (weaponLevels.Count < maxWeapons)
        {
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

    public void UpgradeWeapon(UpgradeableWeapon upgradeableWeapon)
    {
        if (!weaponLevels.ContainsKey(upgradeableWeapon)) return;

        int currentLevel = weaponLevels[upgradeableWeapon];

        if (currentLevel < upgradeableWeapon.MaxLevel)
        {
            weaponLevels[upgradeableWeapon]++;
            Debug.Log($"Arme améliorée : {upgradeableWeapon.weaponLevels[currentLevel + 1].abilityName}");
        }
    }


    public void AddAbility(UpgradeableAbility upgradeableAbility)
    {
        if (!abilityLevels.ContainsKey(upgradeableAbility))
        {
            abilityLevels[upgradeableAbility] = PlayerPrefs.GetInt(upgradeableAbility.name, 0);
        }

        int level = abilityLevels[upgradeableAbility];
        Ability abilityInstance = upgradeableAbility.GetAbilityAtLevel(level);
        if (abilityInstance != null && !activeAbilities.Contains(abilityInstance))
        {
            activeAbilities.Add(abilityInstance);
            Debug.Log($"Nouvelle capacité apprise : {abilityInstance.abilityName}");
        }
    }

    public void UpgradeAbility(UpgradeableAbility upgradeableAbility)
    {
        if (abilityLevels.ContainsKey(upgradeableAbility))
        {
            abilityLevels[upgradeableAbility]++;
            PlayerPrefs.SetInt(upgradeableAbility.name, abilityLevels[upgradeableAbility]);
            PlayerPrefs.Save();
            Debug.Log($"Capacité améliorée : {upgradeableAbility.abilityLevels[0].abilityName} au niveau {abilityLevels[upgradeableAbility]} !");
        }
    }

    private void LoadAbilityLevels()
    {
        foreach (var ability in characterClass.upgradeableAbilities)
        {
            int savedLevel = PlayerPrefs.GetInt(ability.name, 0);
            abilityLevels[ability] = 0;
        }
    }

    public void AddBonus(UpgradeableBonus upgradeableBonus)
    {
        if (bonusLevels.ContainsKey(upgradeableBonus))
        {
            bonusLevels[upgradeableBonus]++;
            Debug.Log($"Amélioration du bonus {upgradeableBonus.bonusLevels[0].bonusName} au niveau {bonusLevels[upgradeableBonus] + 1} !");
        }
        else if (bonusLevels.Count < maxBonuses)
        {
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
        weaponsBonusUI?.UpdateUI(weaponLevels, bonusLevels, abilityLevels);
    }


    //Bonus Methods
    public void IncreaseMaxHealth(float amount) => lifeManager.maxHealth += amount;
    public void Heal(float amount) => lifeManager.Heal(amount);
    public void IncreaseHealthOverTime(float amount, float interval)
    {
        lifeManager.HealthOverTime(amount, interval);
    }
    public void ReduceDamageReceived(float amount)
    {
        lifeManager.reduceDamageValue = amount;
    }

    public void ReviveBonus(int amount)
    {
        lifeManager.AddRespawn(amount);
    }

    public void XPBonus(float value)
    {
        xpBonus = value; //%
    }

    public void IncreaseDamageBonus(float amount) => damageBonus = amount;
    public void IncreaseSpeed(float amount) => speed += amount;

    public float GetMaxHealth() => maxHealth;
    public float GetSpeed() => speed;
    public float GetDamageBonus() => damageBonus;
}



[System.Serializable]
public class DropItem
{
    public GameObject itemPrefab;
    public XpCapsule xpCapsule;
    public bool isChest;
    [Range(0f, 1f)] public float dropChance;
}