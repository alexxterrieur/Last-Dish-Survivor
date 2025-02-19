using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public static PlayerInfos Instance { get; private set; }

    public CharacterClass characterClass;

    private float maxHealth;
    private float speed;
    private float damageBonus;

    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<Ability> activeAbilities = new List<Ability>();
    public List<Bonus> equippedBonuses = new List<Bonus>();

    LifeManager lifeManager;
    AbilityManager abilityManager;
    PlayerMovement playerMovement;

    public enum PlayerDirection { Right, Left, Up, Down }
    public PlayerDirection currentDirection = PlayerDirection.Right;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (characterClass != null)
        {
            InitializeStats();
            EquipStartingWeapons();
        }

        lifeManager = GetComponent<LifeManager>();
        abilityManager = GetComponent<AbilityManager>();
        playerMovement = GetComponent<PlayerMovement>();

        if (lifeManager != null)
        {
            lifeManager.Initialize(maxHealth);
        }

        if (abilityManager != null)
        {
            foreach (Weapon weapon in equippedWeapons)
            {
                abilityManager.AddWeapon(weapon);
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

    public void AddWeapon(Weapon newWeapon)
    {
        if (!equippedWeapons.Contains(newWeapon))
        {
            equippedWeapons.Add(newWeapon);
            Debug.Log($"Nouvelle arme équipée : {newWeapon.abilityName}");

            abilityManager?.AddWeapon(newWeapon);
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

    public void AddBonus(Bonus newBonus)
    {
        if (!equippedBonuses.Contains(newBonus))
        {
            equippedBonuses.Add(newBonus);
            newBonus.ApplyEffect(this);
            Debug.Log($"Nouveau bonus acquis : {newBonus.bonusName}");
        }
    }

    public void IncreaseMaxHealth(float amount) => lifeManager.maxHealth += amount;
    public void IncreaseCurrentHealth(float amount)
    {
        lifeManager.currentHealth += amount;

        if(lifeManager.currentHealth > lifeManager.maxHealth)
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
