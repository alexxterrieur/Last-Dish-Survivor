using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<Ability> activeAbilities = new List<Ability>();

    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();

    void Start()
    {
        foreach (Weapon weapon in PlayerInfos.Instance.equippedWeapons)
        {
            AddWeapon(weapon);
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

    public void AddWeapon(Weapon weapon)
    {
        if (!equippedWeapons.Contains(weapon))
        {
            equippedWeapons.Add(weapon);
            cooldownTimers[weapon] = 0f;
            Debug.Log($"Nouvelle arme ajoutée : {weapon.abilityName}");

            if(weapon.needAttackHandler)
            {
                if (gameObject.GetComponent<AttackHandler>() == null)
                {
                    gameObject.AddComponent<AttackHandler>();
                    Debug.Log("AttackHandler ajouté au joueur");
                }
            }
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
