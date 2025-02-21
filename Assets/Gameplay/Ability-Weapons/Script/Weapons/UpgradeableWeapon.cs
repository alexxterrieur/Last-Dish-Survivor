using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeableWeapon", menuName = "Scriptable Objects/UpgradeableWeapon")]
public class UpgradeableWeapon : ScriptableObject
{
    public List<Weapon> weaponLevels;

    public Weapon GetWeaponAtLevel(int level)
    {
        if (level >= 0 && level < weaponLevels.Count)
            return weaponLevels[level];
        return weaponLevels[weaponLevels.Count - 1];
    }

}
