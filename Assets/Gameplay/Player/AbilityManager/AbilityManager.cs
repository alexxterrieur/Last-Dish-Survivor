using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> equippedWeapons = new List<Ability>();
    public List<Ability> activeAbilities = new List<Ability>();

    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();
    private PlayerInfos playerInfos;

    void Start()
    {
        playerInfos = GetComponent<PlayerInfos>();

        if (playerInfos != null)
        {
            foreach (Weapon weapon in playerInfos.equippedWeapons)
            {
                AddWeapon(weapon);
            }
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

            if (!cooldownTimers.ContainsKey(weapon))
                cooldownTimers[weapon] = 0f;

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

            if (!cooldownTimers.ContainsKey(ability))
                cooldownTimers[ability] = 0f;

            if (Input.GetKeyDown(key) && cooldownTimers[ability] <= 0)
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
            Debug.Log($"Nouvelle arme ajoutée : {weapon.abilityName}");
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
