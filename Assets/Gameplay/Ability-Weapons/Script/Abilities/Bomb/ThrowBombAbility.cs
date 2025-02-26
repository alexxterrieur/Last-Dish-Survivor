using UnityEngine;

[CreateAssetMenu(fileName = "NewThrowBombAbility", menuName = "Scriptable Objects/Abilities/Throw Bomb")]
public class ThrowBombAbility : Weapon
{
    public int numberOfBombs = 1;
    public float timeBetweenBombs = 0f;
    public float projectileSpeed = 5f;

    public override void Activate(GameObject user)
    {
        AttackHandler attackHandler = user.GetComponent<AttackHandler>();
        if (attackHandler != null)
        {
            attackHandler.StartAttack(user, abilityPrefab, numberOfBombs, timeBetweenBombs, cooldown, projectileSpeed, damage);
        }
    }
}
