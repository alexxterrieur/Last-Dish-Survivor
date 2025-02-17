using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Scriptable Objects/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldown;
    public Sprite icon;
    public GameObject abilityPrefab;
    public float duration;
    public float damage;

    protected GameObject lastInstance;

    public virtual void Activate(GameObject user)
    {
        if (abilityPrefab)
        {
            lastInstance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
            Destroy(lastInstance, duration);
        }
    }


    public bool CanActivate(float timer)
    {
        return timer <= 0;
    }
}