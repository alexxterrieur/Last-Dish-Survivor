using UnityEngine;

[CreateAssetMenu(fileName = "BoomerangWeapon", menuName = "Scriptable Objects/Weapon/Boomerang")]
public class BoomerangWeapon : Weapon
{
    public float maxDistance;
    public float timeBeforeReturn;
    public float speed;

    public override void Activate(GameObject user)
    {
        base.Activate(user);

        if (lastInstance == null) return;

        BoomerangProjectile boomerangProjectile = lastInstance.GetComponent<BoomerangProjectile>();
        if (boomerangProjectile != null)
        {
            boomerangProjectile.Initialize(user.transform, maxDistance, timeBeforeReturn, speed, damage + damageBonus);
        }
    }
}
