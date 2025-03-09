using UnityEngine;

[CreateAssetMenu(fileName = "GarlickWeapon", menuName = "Scriptable Objects/Weapon/Garlick")]
public class GarlickWeapon : Weapon
{
    private GameObject garlickInstance;

    public override void Activate(GameObject user)
    {
        if(garlickInstance == null)
        {
            garlickInstance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            garlickInstance.transform.SetParent(user.transform);

            GarlickAura garlickAura = garlickInstance.GetComponent<GarlickAura>();
            garlickAura.damage = damage + damageBonus;
            garlickAura.cooldown = cooldown;
        }
    }
}
