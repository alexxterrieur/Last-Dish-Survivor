using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetStats();
    }

    private void GetStats()
    {
        speed = GetComponent<PlayerInfos>().characterClass.speed;
    }

    private void Update()
    {
        rb.linearVelocity = move;

        if (move.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void MovementPlayer(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        move = new Vector2(input.x, input.y) * speed;
    }
}
