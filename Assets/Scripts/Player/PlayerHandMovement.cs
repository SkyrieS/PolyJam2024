using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float rotationSpeed = 1f;

    private Vector2 _movement;
    private Vector2 _hitVelocity;
    private Quaternion _rotationTarget;

    public Rigidbody2D PlayerRigidbody2D { get { return playerRigidbody2D; } }

    public void UpdateMovement(Vector2 movement)
    {   
        _movement = movement;
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
        else
        {
            _rotationTarget = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        }

        if (_hitVelocity != Vector2.zero)
        {
            _velocity += _hitVelocity;
            _hitVelocity = Vector2.zero;
        }

        playerRigidbody2D.velocity = _velocity;
        RotatePlayerSprite();
    }

    private void RotatePlayerSprite()
    {
        Quaternion rotation = Quaternion.Lerp(transform.rotation, _rotationTarget, Time.deltaTime * rotationSpeed);
        transform.rotation = rotation;
    }
}
