using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterClass", menuName = "Scriptable Objects/Character Class")]
public class CharacterClass : ScriptableObject
{
    public Sprite playerSprite;
    public string className;
    public string classDescription;
    public float maxHealth;
    public float speed;
    public float damageBonus;

    public UpgradeableAbility[] upgradeableAbilities;
    public UpgradeableWeapon[] startWeapon;
}
