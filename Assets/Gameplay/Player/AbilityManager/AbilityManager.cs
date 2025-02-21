using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<UpgradeableWeapon> equippedWeapons = new List<UpgradeableWeapon>();
    public List<Ability> activeAbilities = new List<Ability>();

    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();

    void Start()
    {
        foreach (var weaponEntry in PlayerInfos.Instance.weaponLevels)
        {
            AddWeapon(weaponEntry.Key, weaponEntry.Value);
        }
    }

    void Update()
    {
        HandleWeaponAbilities();
        HandleActiveAbilitiesInput();
    }

    private void HandleWeaponAbilities()
    {
        foreach (var upgradeableWeapon in equippedWeapons)
        {
            int weaponLevel = PlayerInfos.Instance.weaponLevels[upgradeableWeapon];
            Weapon currentWeapon = upgradeableWeapon.GetWeaponAtLevel(weaponLevel);

            if (!cooldownTimers.ContainsKey(currentWeapon))
                cooldownTimers[currentWeapon] = 0f;

            if (cooldownTimers[currentWeapon] <= 0)
            {
                currentWeapon.Activate(gameObject);
                cooldownTimers[currentWeapon] = currentWeapon.cooldown;
            }
            else
            {
                cooldownTimers[currentWeapon] -= Time.deltaTime;
            }
        }
    }

    private void HandleActiveAbilitiesInput()
    {
        for (int i = 0; i < activeAbilities.Count; i++)
        {
            Ability ability = activeAbilities[i];
            KeyCode key = KeyCode.Alpha1 + i;

            if (!cooldownTimers.ContainsKey(ability))
                cooldownTimers[ability] = 0f;

            if (Input.GetKeyDown(key) && cooldownTimers[ability] <= 0)
            {
                ability.Activate(gameObject);
                cooldownTimers[ability] = ability.cooldown;
            }
        }
    }

    public void AddWeapon(UpgradeableWeapon upgradeableWeapon, int level)
    {
        if (!equippedWeapons.Contains(upgradeableWeapon))
        {
            equippedWeapons.Add(upgradeableWeapon);
            Debug.Log($"Nouvelle arme ajoutée : {upgradeableWeapon.weaponLevels[level].abilityName}");
        }
        else
        {
            Debug.Log($"Arme améliorée : {upgradeableWeapon.weaponLevels[level].abilityName} Niveau {level}");
        }

        Weapon currentWeapon = upgradeableWeapon.GetWeaponAtLevel(level);
        cooldownTimers[currentWeapon] = 0f;

        if (currentWeapon.needAttackHandler && gameObject.GetComponent<AttackHandler>() == null)
        {
            gameObject.AddComponent<AttackHandler>();
            Debug.Log("AttackHandler ajouté au joueur");
        }
    }


    public void AddActiveAbility(Ability ability)
    {
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            cooldownTimers[ability] = 0f;
            Debug.Log($"Nouvelle capacité ajoutée : {ability.abilityName}");
        }
    }
}
