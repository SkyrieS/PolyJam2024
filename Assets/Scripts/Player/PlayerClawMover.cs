using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Items;
using UnityEngine;

public class PlayerClawMover : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRb;
    [SerializeField] private Transform clawTransform;
    [SerializeField] private Transform raycastPos;
    [SerializeField] private List<Collider2D> colliders;
    [SerializeField] private StompAnim stompAnimExample;
    [SerializeField] private ExploAnim exploAn;
    [SerializeField] private PlayerTag tag;
    [SerializeField] private float raycastLenghth;
    [SerializeField] private float openAngle;
    [SerializeField] private float closedAngle;
    [SerializeField] private float closingSpeed;
    [SerializeField] private float dropForce;
    [SerializeField] private float explosionCooldown;

    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;

    private float explosionProgress;
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
        exploAn.ProcessClock(explosionProgress / explosionCooldown);

        if (currentlyHolded != null)
            return;

        HandleExplosion();

        if(_isClosing && currentlyHolded == null && !comingBack)
        {
            DetectItemGrab();
        }

        float angle;
        if (_isClosing)
        {
            angle = Mathf.Lerp(_currentAngle, closedAngle, Time.deltaTime * closingSpeed);
        }
        else
        {
            angle = Mathf.Lerp(_currentAngle, openAngle, Time.deltaTime * closingSpeed);
            exploded = false;
        }

        _currentAngle = angle;
        clawTransform.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void HandleExplosion()
    {
        if (exploded || !_isClosing)
            return;

        explosionProgress += Time.deltaTime;
        float progress = explosionProgress / explosionCooldown;

        if(progress >= 1f)
        {
            Explode();
        }
    }

    public void Explode()
    {
        exploded = true;
        explosionProgress = 0;
        Debug.Log("Explode");
        var stomp = Instantiate(stompAnimExample);
        stomp.SetTargetSize(new Vector3(explosionRadius, explosionRadius, explosionRadius));
        stomp.transform.position = transform.position;
        stomp.gameObject.SetActive(true);

        var colls = Physics2D.OverlapCircleAll(transform.position, explosionRadius).ToList();
        Debug.Log(colls.RemoveAll(c => c.transform.tag == "Player"));
        foreach(var col in colls)
        {
            RigibodyLinker rb = col.GetComponent<RigibodyLinker>();
            if(rb != null && rb != myRb)
            {
                Vector2 dir = (Vector2)rb.transform.position - (Vector2)transform.position;
                float dist = dir.magnitude;
                rb.rb.AddForce(Mathf.Lerp(0, explosionForce, Mathf.InverseLerp(0, explosionRadius, dist)) * dir, ForceMode2D.Impulse);
   
                BaseItem item = rb.rb.gameObject.GetComponent<BaseItem>();
                if(item != null)
                {
                    item.currentTag = tag;
                }
            }
        }
    }

    public void DetectItemGrab()
    {
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

        if(currentlyHolded.Joint == null)
        {
            currentlyHolded = null;
            return;
        }

        currentlyHolded.Joint.enabled = false;
        currentlyHolded.Joint.connectedBody = null;
        
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

        if (!isClosing)
        {
            exploded = false;
            explosionProgress = 0f;
        }
            
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(raycastPos.position, raycastPos.transform.up * raycastLenghth);
    }
}

[System.Serializable]
public class ExploAnim
{
    public SpriteRenderer rend;
    public Vector3 startSize;
    public Vector3 targetSize;
    public Gradient gradient;
    public float speed;

    private float currentProcess;

    public void ProcessClock(float targetProcess)
    {
        currentProcess = Mathf.Lerp(currentProcess, targetProcess, Time.deltaTime * speed);
        Vector3 scale = Vector3.Lerp(startSize, targetSize, currentProcess);
        rend.transform.localScale = scale;
        rend.color = gradient.Evaluate(currentProcess);
    }
}