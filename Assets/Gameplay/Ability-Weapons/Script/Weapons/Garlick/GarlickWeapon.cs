using UnityEngine;

[CreateAssetMenu(fileName = "GarlickWeapon", menuName = "Scriptable Objects/Weapon/Garlick")]
public class GarlickWeapon : Weapon
{
    private GameObject garlickInstance;
    public float newRadiusSize;

    public override void Activate(GameObject user)
    {
        if (garlickInstance == null)
        {
            garlickInstance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            garlickInstance.transform.SetParent(user.transform);
        }

        // Mise à jour de l'arme existante
        GarlickAura garlickAura = garlickInstance.GetComponent<GarlickAura>();
        if (garlickAura != null)
        {
            garlickAura.UpdateStats(damage + damageBonus, cooldown, newRadiusSize);
        }
    }
}
