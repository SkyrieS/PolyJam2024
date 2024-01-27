using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform handsRbParent;
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 5f;

    private Vector2 _movement;
    private Vector2 _hitVelocity;

    public Rigidbody2D PlayerRigidbody2D { get { return playerRigidbody2D; } }

    public void UpdateMovement(InputAction.CallbackContext obj)
    {   
        _movement = obj.ReadValue<Vector2>();
    }

    public void AddVelocity(Vector2 direction)
    {
        _hitVelocity = direction;
    }

    private void Update()
    {
        Vector2 direction = _movement.normalized;

        Vector2 _velocity = playerRigidbody2D.velocity;
        _velocity += direction * acceleration * Time.deltaTime;
        _velocity = Vector2.ClampMagnitude(_velocity, maxSpeed);

        if (direction == Vector2.zero)
        {
            _velocity = Vector2.Lerp(_velocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        if(_hitVelocity != Vector2.zero)
        {
            _velocity += _hitVelocity;
            _hitVelocity = Vector2.zero;
        }

        playerRigidbody2D.velocity = _velocity;

        RotatePlayerSprite(_velocity);
    }

    private void FixedUpdate()
    {
        handsRbParent.position = this.transform.position;
        handsRbParent.rotation = this.transform.rotation;
    }

    private void RotatePlayerSprite(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }
}
