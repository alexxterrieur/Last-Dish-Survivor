using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public CharacterClass characterClass;

    private float currentHealth;
    private float currentShield;
    private float speed;
    private float damageBonus;

    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<Ability> activeAbilities = new List<Ability>();
    public List<Bonus> equippedBonuses = new List<Bonus>();

    void Start()
    {
        if (characterClass != null)
        {
            InitializeStats();
            EquipStartingWeapons();
        }
    }

    private void InitializeStats()
    {
        currentHealth = characterClass.maxHealth;
        currentShield = characterClass.maxShield;
        speed = characterClass.speed;
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

    public void IncreaseHealth(float amount) => currentHealth += amount;
    public void IncreaseShield(float amount) => currentShield += amount;
    public void IncreaseDamageBonus(float amount) => damageBonus += amount;
    public void IncreaseSpeed(float amount) => speed += amount;


    public float GetSpeed() => speed;
    public float GetDamageBonus() => damageBonus;
    public float GetCurrentHealth() => currentHealth;
    public float GetCurrentShield() => currentShield;
}
