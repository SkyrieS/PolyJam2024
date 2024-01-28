using System.Collections;
using System.Collections.Generic;
using Game.Items;
using UnityEngine;

public class PlayerClawMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRb;
    [SerializeField] private Transform clawTransform;
    [SerializeField] private Transform raycastPos;
    [SerializeField] private List<Collider2D> colliders;
    [SerializeField] private float raycastLenghth;
    [SerializeField] private float openAngle;
    [SerializeField] private float closedAngle;
    [SerializeField] private float closingSpeed;
    [SerializeField] private float dropForce;

    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private bool _isClosing;
    private float _currentAngle;
    private bool exploded = false;
    public bool comingBack;

    public IPickable currentlyHolded;
    public Transform holderPrev;

    private void Awake()
    {
        _isClosing = false;
        _currentAngle = openAngle;
    }

    private void Update()
    {
        if (currentlyHolded != null)
            return;

        if(_isClosing && currentlyHolded == null && !comingBack)
        {
            DetectItemGrab();
        }

        float angle;
        if (_isClosing)
        {
            angle = Mathf.Lerp(_currentAngle, closedAngle, Time.deltaTime * closingSpeed);
            if(Mathf.Abs(angle - closedAngle) < 0.5f && !exploded)
            {
                //Explode();
                exploded = true;
            }
        }
        else
        {
            angle = Mathf.Lerp(_currentAngle, openAngle, Time.deltaTime * closingSpeed);
            exploded = false;
        }

        _currentAngle = angle;
        clawTransform.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Explode()
    {
        Debug.Log("Explode");
        var colls = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(var col in colls)
        {
            RigibodyLinker rb = col.GetComponent<RigibodyLinker>();
            if(rb != null && rb != myRb)
            {
                Debug.Log($"{col} and {rb}");
                Vector2 dir = (Vector2)rb.transform.position - (Vector2)transform.position;
                float dist = dir.magnitude;
                rb.rb.AddForce(Mathf.Lerp(0, explosionForce, Mathf.InverseLerp(0, explosionRadius, dist)) * dir, ForceMode2D.Impulse);
                Debug.Log(Mathf.Lerp(0, explosionForce, Mathf.InverseLerp(0, explosionRadius, dist)) * dir);
            }
        }
    }

    public void DetectItemGrab()
    {
        Debug.Log("Detec");
        RaycastHit2D hit = Physics2D.Raycast(raycastPos.position, raycastPos.transform.up, raycastLenghth);
        
        if (hit.transform == null)
            return;

        RigibodyLinker rbPicker = hit.transform.GetComponent<RigibodyLinker>();

        if (rbPicker == null)
            return;

        IPickable item = rbPicker.rb.transform.GetComponent<IPickable>();

        if(item != null)
        {
            Debug.Log("ItemFound" + item.transform.name, item.transform.gameObject);
            this.currentlyHolded = item;
            holderPrev = currentlyHolded.transform;
            item.Joint.enabled = true;
            item.Joint.connectedBody = myRb;
            currentlyHolded.PickItem();
            currentlyHolded.OnGrabbed += OnOtherPlayerGrabbed;
        }
    }

    public void Drop()
    {
        if (currentlyHolded == null)
            return;

        currentlyHolded.Joint.connectedBody = null;
        currentlyHolded.Joint.enabled = false;

        foreach (var col in colliders)
            col.enabled = false;

        Invoke("EnableColldiers", .1f);
        currentlyHolded.Rigidbody2D.AddForce(transform.right * dropForce, ForceMode2D.Impulse);
        currentlyHolded.OnGrabbed -= OnOtherPlayerGrabbed;
        currentlyHolded = null;
    }

    public void EnableColldiers()
    {
        foreach (var col in colliders)
            col.enabled = true;
    }

    public void OnOtherPlayerGrabbed()
    {
        currentlyHolded.OnGrabbed -= OnOtherPlayerGrabbed;
        currentlyHolded = null;
    }

    public void ToggleClosing(bool isClosing)
    {
        _isClosing = isClosing;
        colliders[0].enabled = !isClosing;
        if (!_isClosing && currentlyHolded != null)
            Drop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(raycastPos.position, raycastPos.transform.up * raycastLenghth);

    }
}
