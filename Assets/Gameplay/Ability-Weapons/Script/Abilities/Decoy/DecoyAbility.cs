using UnityEngine;

[CreateAssetMenu(fileName = "DecoyAbility", menuName = "Scriptable Objects/Ability/Attack/Decoy")]
public class DecoyAbility : Ability
{
    [SerializeField] private float decoySpeed = 3f;

    public override void Activate(GameObject user)
    {
        if (abilityPrefab == null) return;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        //GameObject decoyInstance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
        base.Activate(user);

        Decoy decoyScript = lastInstance.GetComponent<Decoy>();
        if (decoyScript != null)
        {
            decoyScript.Initialize(randomDirection, decoySpeed);
        }
    }
}
