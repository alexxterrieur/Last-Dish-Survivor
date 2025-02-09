using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Scriptable Objects/Character Class")]
public class CharacterClass : ScriptableObject
{
    public string className;
    public float maxHealth;
    public float maxShield;
    public float speed;
    public float damageBonus;

    public Ability[] activeAbilities;
    public Weapon[] startWeapon;
}
