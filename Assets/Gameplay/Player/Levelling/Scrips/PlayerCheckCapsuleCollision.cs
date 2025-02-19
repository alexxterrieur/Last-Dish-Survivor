using UnityEngine;

public class PlayerCheckCapsuleCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        XPCapsuleBehavior xPCapsuleBehavior = collision.GetComponent< XPCapsuleBehavior>();
        if(xPCapsuleBehavior != null )
        {
            xPCapsuleBehavior.AttractToPlayer(gameObject.transform.parent);
        }
    }
}
