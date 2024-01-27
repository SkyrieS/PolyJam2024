using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClawMover : MonoBehaviour
{
    [SerializeField] private Transform clawTransform;
    [SerializeField] private float openAngle;
    [SerializeField] private float closedAngle;
    [SerializeField] private float closingSpeed;

    private bool _isClosing;
    private float _currentAngle;

    private void Awake()
    {
        _isClosing = false;
        _currentAngle = openAngle;
    }

    private void Update()
    {
        float angle;
        if (_isClosing)
        {
            angle = Mathf.Lerp(_currentAngle, closedAngle, Time.deltaTime * closingSpeed);
        }
        else
        {
            angle = Mathf.Lerp(_currentAngle, openAngle, Time.deltaTime * closingSpeed);
        }
        _currentAngle = angle;
        clawTransform.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ToggleClosing(bool isClosing)
    {
        _isClosing = isClosing;
    }
}
