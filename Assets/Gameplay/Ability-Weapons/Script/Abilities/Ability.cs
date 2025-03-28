using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Scriptable Objects/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldown;
    public Sprite icon;
    public string description;
    public GameObject abilityPrefab;
    public float duration;
    public float damage;
    public float damageBonus;

    public bool waitBeforeCooldown;
    protected GameObject lastInstance;

    public virtual void Activate(GameObject user)
    {
        //if (abilityPrefab)
        //{
        //    damageBonus = PlayerInfos.Instance.GetDamageBonus();

        //    lastInstance = Instantiate(abilityPrefab, user.transform.position, Quaternion.identity);
        //    Destroy(lastInstance, duration);
        //}

        if (abilityPrefab == null) return;

        string poolKey = abilityPrefab.name + "(Clone)";
        lastInstance = PoolingManager.Instance.GetFromPool(poolKey, user.transform.position, Quaternion.identity);

        if (lastInstance != null)
        {
            damageBonus = PlayerInfos.Instance.GetDamageBonus();
            user.GetComponent<MonoBehaviour>().StartCoroutine(ReturnToPoolAfterDuration(lastInstance, duration));
        }
    }


    public bool CanActivate(float timer)
    {
        return timer <= 0;
    }

    private IEnumerator ReturnToPoolAfterDuration(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (instance.activeInHierarchy)
        {
            PoolingManager.Instance.ReturnToPool(instance.name, instance);
        }
    }

}
