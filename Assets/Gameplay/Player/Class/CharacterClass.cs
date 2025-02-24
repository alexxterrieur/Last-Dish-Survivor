using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Scriptable Objects/Character Class")]
public class CharacterClass : ScriptableObject
{
    public Sprite playerSprite;
    public string className;
    public float maxHealth;
    public float speed;
    public float damageBonus;

    public Ability[] activeAbilities;
    public UpgradeableWeapon[] startWeapon;
}
