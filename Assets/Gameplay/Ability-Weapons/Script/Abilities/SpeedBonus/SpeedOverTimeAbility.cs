using UnityEngine;

[CreateAssetMenu(fileName = "SpeedAbility", menuName = "Scriptable Objects/Ability/Rotation/SpeedOverTime")]
public class SpeedOverTimeAbility : Ability
{
    public float newSpeed;

    public override void Activate(GameObject user)
    {
        PlayerMovement playerMovement = user.GetComponent<PlayerMovement>();
        playerMovement.PerformSpeedAbilityOverTime(this, newSpeed, duration);
    }
}
