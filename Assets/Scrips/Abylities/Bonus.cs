using UnityEngine;

[CreateAssetMenu(fileName = "NewBonus", menuName = "Scriptable Objects/Bonus")]
public class Bonus : ScriptableObject
{
    public string bonusName;
    public BonusType type;
    public float value;

    public void ApplyEffect(PlayerInfos player)
    {
        switch (type)
        {
            case BonusType.IncreaseHealth:
                player.IncreaseHealth(value);
                break;
            case BonusType.IncreaseShield:
                player.IncreaseShield(value);
                break;
            case BonusType.IncreaseDamage:
                player.IncreaseDamageBonus(value);
                break;
            case BonusType.IncreaseSpeed:
                player.IncreaseSpeed(value);
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
    IncreaseShield,
    IncreaseDamage,
    IncreaseSpeed
}
