using UnityEngine;

[CreateAssetMenu(fileName = "PoisonZoneAbility", menuName = "Scriptable Objects/Ability/Attack/Poison Zone")]
public class PoisonZoneAbility : Ability
{
    public float effectDuration;
    
    public override void Activate(GameObject user)
    {
        if (abilityPrefab)
        {
            GameObject poisonZone = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            PoisonZone abilityScript = poisonZone.GetComponent<PoisonZone>();
            
            abilityScript.effectDuration = effectDuration;
            abilityScript.damagePerTick = damage;

            Destroy(poisonZone, duration);
        }
    }
}
