using UnityEngine;

[CreateAssetMenu(fileName = "PiercingDashAbility", menuName = "Scriptable Objects/Ability/PiercingDash")]
public class PiercingDashAbility : Ability
{
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;

    public override void Activate(GameObject user)
    {
        PlayerMovement playerMovement = user.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.PerformPiercingDash(dashDistance, dashSpeed, damage);
        }
    }
}
