using UnityEngine;

[CreateAssetMenu(fileName = "SmokeZoneAbility", menuName = "Scriptable Objects/Ability/Attack/Smoke Zone")]
public class SmokeZoneAbility : Ability
{
    public override void Activate(GameObject user)
    {
        if (abilityPrefab)
        {
            //GameObject poisonZone = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            base.Activate(user);

            Smoke abilityScript = lastInstance.GetComponent<Smoke>();

            abilityScript.duration = duration;
            abilityScript.damagePerTick = damage;

            
            //Destroy(poisonZone, duration);
        }
    }
}
