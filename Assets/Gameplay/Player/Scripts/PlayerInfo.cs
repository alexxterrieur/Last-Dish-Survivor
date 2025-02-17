using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public CharacterClass characterClass;

    private float maxHealth;
    private float speed;
    private float damageBonus;

    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<Ability> activeAbilities = new List<Ability>();
    public List<Bonus> equippedBonuses = new List<Bonus>();

    LifeManager lifeManager;

    void Start()
    {
        if (characterClass != null)
        {
            InitializeStats();
            EquipStartingWeapons();
        }

        lifeManager = GetComponent<LifeManager>();
        if (lifeManager != null)
        {
            lifeManager.Initialize(maxHealth);
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
