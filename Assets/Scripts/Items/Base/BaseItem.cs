using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class BaseItem : MonoBehaviour, IPickable
    {
        [SerializeField] private Rigidbody2D itemRigidbody2D;
        [SerializeField] private Collider2D itemCollider;
        public Rigidbody2D Rigidbody2D => itemRigidbody2D;
        public GameObject Object => this.gameObject;

        private bool isHeld;
        public bool IsHeld => isHeld;

        private IPlayer parentPlayer;
        public IPlayer ParentPlayer => parentPlayer;

        public virtual void PickItem(IPlayer player, HandType handType)
        {
            itemRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            itemCollider.enabled = false;
            isHeld = true;
            parentPlayer = player;
            player.AddItemToHand(this, handType);
        }

        public virtual void DropItem()
        {
            itemRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            itemCollider.enabled = true;
            isHeld = false;
            parentPlayer = null;
        }
    }
}