using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float alphaSet = 0.8f;
    private float alphaMultiplayer = 0.85f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRenderer;

    private Color color;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        if(playerSpriteRenderer.flipX)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRenderer.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplayer;
        color = new Color(1f,1f, 1f, alpha);
        spriteRenderer.color = color;

        if(Time.time > timeActivated + activeTime)
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
