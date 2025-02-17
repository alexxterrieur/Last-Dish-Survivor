using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Scriptable Objects/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldown;
    public Sprite icon;
    public GameObject abilityPrefab;
    public float range;
    public float duration;

    public virtual void Activate(GameObject user)
    {
        Debug.Log($"{abilityName} activée par {user.name}");
        if (abilityPrefab)
        {
            GameObject instance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            Destroy(instance, duration);
        }
    }

    public bool CanActivate(float timer)
    {
        return timer <= 0;
    }
}