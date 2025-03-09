using UnityEngine;

[CreateAssetMenu(fileName = "NewBonus", menuName = "Scriptable Objects/Bonus")]
public class Bonus : ScriptableObject
{
    public string bonusName;
    public Sprite icon;
    public string description;
    public BonusType type;
    public float value;
    public float repetitionInterval;
    public void ApplyEffect(PlayerInfos player)
    {
        switch (type)
        {
            case BonusType.IncreaseHealth:
                player.Heal(value);
                break;
            case BonusType.IncreaseHealthOverTime:
                player.IncreaseHealthOverTime(value, repetitionInterval);
                break;
            case BonusType.IncreaseMaxHealth:
                player.IncreaseMaxHealth(value);
                break;
            case BonusType.IncreaseDamage:
                player.IncreaseDamageBonus(value);
                break;
            case BonusType.IncreaseSpeed:
                player.IncreaseSpeed(value);
                break;
            case BonusType.ReduceDamageReceived:
                player.ReduceDamageReceived(value);
                break;
            case BonusType.Revive:
                player.ReviveBonus();
                break;
            default:
                Debug.LogWarning("unknown item");
                break;
        }
    }
}

public enum BonusType
{
    IncreaseHealth,
    IncreaseHealthOverTime,
    IncreaseMaxHealth,
    IncreaseDamage,
    IncreaseSpeed,
    ReduceDamageReceived,
    Revive
}
