using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> equippedWeapons = new List<Ability>(); // Armes auto
    public List<Ability> activeAbilities = new List<Ability>(); // Capacités manuelles

    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();

    void Start()
    {
        foreach (var ability in equippedWeapons)
        {
            cooldownTimers[ability] = 0f;
        }
    }

    void Update()
    {
        HandleWeaponAbilities();
        HandleActiveAbilitiesInput();
    }

    private void HandleWeaponAbilities()
    {
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            Ability weapon = equippedWeapons[i];

            if (cooldownTimers[weapon] <= 0)
            {
                weapon.Activate(gameObject);
                cooldownTimers[weapon] = weapon.cooldown;
            }
            else
            {
                cooldownTimers[weapon] -= Time.deltaTime;
            }
        }
    }

    private void HandleActiveAbilitiesInput()
    {
        for (int i = 0; i < activeAbilities.Count; i++)
        {
            Ability ability = activeAbilities[i];
            KeyCode key = KeyCode.Alpha1 + i; // Ex: 1, 2, 3 pour les capacités actives

            if (Input.GetKeyDown(key) && cooldownTimers.GetValueOrDefault(ability, 0) <= 0)
            {
                ability.Activate(gameObject);
                cooldownTimers[ability] = ability.cooldown;
            }
        }
    }

    public void AddWeapon(Ability weapon)
    {
        if (!equippedWeapons.Contains(weapon))
        {
            equippedWeapons.Add(weapon);
            cooldownTimers[weapon] = 0f;
        }
    }

    public void AddActiveAbility(Ability ability)
    {
        if (!activeAbilities.Contains(ability))
        {
            activeAbilities.Add(ability);
            cooldownTimers[ability] = 0f;
        }
    }
}
