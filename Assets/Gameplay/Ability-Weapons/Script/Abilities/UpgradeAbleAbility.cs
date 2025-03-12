using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeableAbility", menuName = "Scriptable Objects/Upgradeable/Upgradeable Ability")]
public class UpgradeableAbility : ScriptableObject
{
    public List<Ability> abilityLevels;

    public int abilityIndex;
    public string abilityDescription;

    public Ability GetAbilityAtLevel(int level)
    {
        if (level < 0 || level >= abilityLevels.Count)
            return abilityLevels[abilityLevels.Count - 1];
        return abilityLevels[level];
    }
}
