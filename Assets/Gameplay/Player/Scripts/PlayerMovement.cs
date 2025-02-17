using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _move;
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;

    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        GetStats();
    }

    private void GetStats()
    {
        _speed = GetComponent<PlayerInfos>().characterClass.speed;
    }

    private void Update()
    {
        _rb.linearVelocity = _move;
    }

    public void MovementPlayer(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        _move = new Vector2(input.x, input.y) * _speed;
    }
}
