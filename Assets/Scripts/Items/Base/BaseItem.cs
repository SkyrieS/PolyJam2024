using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Items
{
    public class BaseItem : MonoBehaviour, IPickable
    {
        [SerializeField] private Rigidbody2D itemRigidbody2D;
        [SerializeField] private Collider2D itemCollider;
        [SerializeField] private FixedJoint2D joint;

        public FixedJoint2D Joint => joint;

        private int item_id;
        public int Item_id { get { return item_id; } set {  item_id = value; } }

        [Header("Tesing")]
        public Vector3 someForce;
        public float torque;

        public Collider2D ItemCollider => itemCollider;
        public Rigidbody2D Rigidbody2D => itemRigidbody2D;

        public bool IsHeld { get; set; }
        UnityAction IPickable.OnGrabbed { get; set; }

        public PlayerTag currentTag;

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision", collision.rigidbody.gameObject);
            Debug.Log(collision.transform.GetComponent<PlayerTag>());
            currentTag = collision.transform.GetComponent<PlayerTag>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AddForce(someForce, torque);
        }

        public void AddForce(Vector2 force, float torque)
        {
            itemRigidbody2D.AddForce(force, ForceMode2D.Impulse);
            itemRigidbody2D.AddTorque(torque, ForceMode2D.Impulse);
        }
    }
}