using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "Scriptable Objects/Ability/Rotation/Dash")]
public class DashAbility : Ability
{
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;

    public override void Activate(GameObject user)
    {
        PlayerMovement playerMovement = user.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.PerformDash(dashDistance, dashSpeed);
        }
    }
}
