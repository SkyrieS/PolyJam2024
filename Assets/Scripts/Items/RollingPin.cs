using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class RollingPin : BaseItem
    {
        [SerializeField] private float hitStrenght;
        [SerializeField] private Collider2D pickupCollider;
        [SerializeField] private Collider2D hitCollider;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsHeld)
            {
                if (collision.TryGetComponent(out PlayerMovement playerController))
                {
                    Vector2 direction = (transform.position - collision.transform.position).normalized;
                    playerController.AddVelocity(-direction * hitStrenght);
                }
            }
        }

        public override void PickItem(IPlayer player, HandType handType)
        {
            base.PickItem(player, handType);
            pickupCollider.enabled = false;
            hitCollider.enabled = true;
        }

        public override void DropItem()
        {
            base.DropItem();
            pickupCollider.enabled = true;
            hitCollider.enabled = false;
        }
    } 
}
