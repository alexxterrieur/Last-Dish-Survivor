using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeableBonus", menuName = "Scriptable Objects/Upgradeable/Upgradeable Bonus")]
public class UpgradeableBonus : ScriptableObject
{
    public List<Bonus> bonusLevels;

    public Bonus GetBonusAtLevel(int level)
    {
        if (level >= 0 && level < bonusLevels.Count)
            return bonusLevels[level];
        return bonusLevels[bonusLevels.Count - 1];
    }
}
