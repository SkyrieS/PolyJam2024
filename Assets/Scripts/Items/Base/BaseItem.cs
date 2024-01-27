using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class BaseItem : MonoBehaviour, IPickable
    {
        [SerializeField] private Rigidbody2D itemRigidbody2D;
        public Rigidbody2D Rigidbody2D => itemRigidbody2D;
        public GameObject Object => this.gameObject;

        private bool isHeld;
        public bool IsHeld => isHeld;

        public virtual void PickItem(IPlayer player, HandType handType)
        {
            itemRigidbody2D.simulated = false;
            isHeld = true;
            player.AddItemToHand(this, handType);
        }

        public virtual void DropItem()
        {
            itemRigidbody2D.simulated = true;
            isHeld = false;
        }
    }
}