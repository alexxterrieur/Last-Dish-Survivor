using UnityEngine;

public class XPCapsuleBehavior : MonoBehaviour
{
    public XpCapsule capsuleScriptable;
    private Transform playerTransform;
    public bool isAttracted = false;
    public float attractionSpeed;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = capsuleScriptable.sprite;    
    }

    private void Update()
    {
        if (isAttracted && playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if(distanceToPlayer <= 0.5f)
        {
            playerTransform.gameObject.GetComponent<PlayerLevelManager>().AddXP(capsuleScriptable.xpValue);
            Destroy(gameObject);
        }
    }

    public void AttractToPlayer(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        isAttracted = true;
    }
}
