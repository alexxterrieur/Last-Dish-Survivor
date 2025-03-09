using UnityEngine;

[CreateAssetMenu(fileName = "WhipWeapon", menuName = "Scriptable Objects/Weapon/Whip")]
public class WhipWeapon : Weapon
{
    private GameObject whipInstance;
    public float delayBetweenCollider;

    public override void Activate(GameObject user)
    {
        if (whipInstance == null)
        {
            whipInstance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            whipInstance.transform.SetParent(user.transform);

            WhipBehavior whipBehavior = whipInstance.GetComponent<WhipBehavior>();
            whipBehavior.damage = damage + damageBonus;
            whipBehavior.cooldown = cooldown;
            whipBehavior.colliderCooldown = delayBetweenCollider;

            whipBehavior.Initialize(user);
        }
    }
}