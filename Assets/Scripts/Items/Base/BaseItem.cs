using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class BaseItem : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D itemRigidbody2D;
        [SerializeField] private Collider2D itemCollider;

        [Header("Tesing")]
        public Vector3 someForce;
        public float torque;

        public Collider2D ItemCollider => itemCollider;
        public Rigidbody2D Rigidbody2D => itemRigidbody2D;

        public List<PlayerTag> currentTags;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerTag tag = collision.transform.GetComponent<PlayerTag>();
            if (tag != null && !currentTags.Contains(tag))
                currentTags.Add(tag);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            PlayerTag tag = collision.transform.GetComponent<PlayerTag>();
            if (tag != null)
                currentTags.Remove(tag);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AddForce();
        }

        public void AddForce()
        {
            itemRigidbody2D.AddForce(someForce, ForceMode2D.Impulse);
            itemRigidbody2D.AddTorque(torque, ForceMode2D.Impulse);
        }
    }
}