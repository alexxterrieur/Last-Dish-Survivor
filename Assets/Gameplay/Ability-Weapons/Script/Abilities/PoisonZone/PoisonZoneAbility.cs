using UnityEngine;

[CreateAssetMenu(fileName = "PoisonZoneAbility", menuName = "Scriptable Objects/Ability/Attack/Poison Zone")]
public class PoisonZoneAbility : Ability
{
    public float effectDuration;
    
    public override void Activate(GameObject user)
    {
        if (abilityPrefab)
        {
            //GameObject poisonZone = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            base.Activate(user);

            PoisonZone abilityScript = lastInstance.GetComponent<PoisonZone>();
            
            abilityScript.effectDuration = effectDuration;
            abilityScript.damagePerTick = damage + damageBonus;

            //Destroy(poisonZone, duration);
        }
    }

    private void ReturnToPool()
    {
        PoolingManager.Instance.ReturnToPool(lastInstance.name, lastInstance);
    }
}
