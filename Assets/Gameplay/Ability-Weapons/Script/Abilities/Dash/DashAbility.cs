using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "Scriptable Objects/Ability/Dash")]
public class DashAbility : Ability
{
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashSpeed;
    [SerializeField] private int maxDashCount;

    private int currentDashCount = 0;

    public override void Activate(GameObject user)
    {
        if (currentDashCount < maxDashCount)
        {
            PlayerMovement playerMovement = user.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.PerformDash(dashDistance, dashSpeed);
                currentDashCount++;
            }
        }
    }

    public void ResetDashCount()
    {
        currentDashCount = 0;
    }
}
