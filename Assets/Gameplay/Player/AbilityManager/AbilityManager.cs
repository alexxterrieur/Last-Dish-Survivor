using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    public List<UpgradeableWeapon> equippedWeapons = new List<UpgradeableWeapon>();
    public List<Ability> activeAbilities = new List<Ability>();

    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();

    private void Start()
    {
        foreach (var weaponEntry in PlayerInfos.Instance.weaponLevels)
        {
            AddWeapon(weaponEntry.Key, weaponEntry.Value);
        }
    }

    private void Update()
    {
        HandleWeaponAbilities();
        UpdateCooldowns();
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
        }
    }

    private void UpdateCooldowns()
    {
        List<Ability> keys = new List<Ability>(cooldownTimers.Keys);
        foreach (var ability in keys)
        {
            if (cooldownTimers[ability] > 0)
            {
                cooldownTimers[ability] -= Time.deltaTime;
            }
        }
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseAbility(0);
            Debug.Log("ability 1 used");
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (context.performed) UseAbility(1);
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (context.performed) UseAbility(2);
    }

    public void OnAbility4(InputAction.CallbackContext context)
    {
        if (context.performed) UseAbility(3);
    }

    private void UseAbility(int abilityIndex)
    {
        if (abilityIndex >= activeAbilities.Count) return;

        Ability ability = activeAbilities[abilityIndex];

        if (!cooldownTimers.ContainsKey(ability) || cooldownTimers[ability] <= 0)
        {
            ability.Activate(gameObject);
            cooldownTimers[ability] = ability.cooldown;
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
