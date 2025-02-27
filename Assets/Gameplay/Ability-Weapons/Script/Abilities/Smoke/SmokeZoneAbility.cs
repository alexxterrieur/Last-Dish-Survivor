using UnityEngine;

[CreateAssetMenu(fileName = "SmokeZoneAbility", menuName = "Scriptable Objects/Abilities/Smoke Zone")]
public class SmokeZoneAbility : Ability
{
    public override void Activate(GameObject user)
    {
        if (abilityPrefab)
        {
            GameObject poisonZone = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            Smoke abilityScript = poisonZone.GetComponent<Smoke>();

            abilityScript.duration = duration;
            abilityScript.damagePerTick = damage;

            Destroy(poisonZone, duration);
        }
    }
}
