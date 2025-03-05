using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeableWeapon", menuName = "Scriptable Objects/Upgradeable/Upgradeable Weapon")]
public class UpgradeableWeapon : ScriptableObject
{
    public List<Weapon> weaponLevels = new List<Weapon>();
    public UpgradeableBonus requiredBonusForLegendary;

    public int MaxLevel => weaponLevels.Count - 2;
    public bool HasLegendary => weaponLevels.Count > 1;

    public Weapon GetWeaponAtLevel(int level)
    {
        return weaponLevels[Mathf.Clamp(level, 0, weaponLevels.Count - 1)];
    }
}
