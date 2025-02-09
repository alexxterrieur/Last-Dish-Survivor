using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : Ability
{
    public override void Activate(GameObject user)
    {
        Debug.Log($"Arme {abilityName} tir�e automatiquement !");
        base.Activate(user);
    }
}
