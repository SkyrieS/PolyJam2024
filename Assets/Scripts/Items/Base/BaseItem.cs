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

        public Rigidbody2D Rigidbody2D => itemRigidbody2D;
        public Collider2D ItemCollider => itemCollider;

        public virtual void PickItem(IPlayer player, HandType handType)
        {
            itemRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            itemCollider.enabled = false;
        }

        public virtual void DropItem()
        {
            itemRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            itemCollider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            
        }
    }
}