using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class RollingPinItem : BaseItem
    {
        [SerializeField] private float hitStrenght;
        [SerializeField] private Collider2D pickupTriggerCollider;
        [SerializeField] private Collider2D hitTriggerCollider;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (IsHeld)
            //{
                //if (collision.TryGetComponent(out IPlayer player) && player != ParentPlayer)
                //{
                //    Vector2 direction = (transform.position - collision.transform.position).normalized;
                //    player.AddVelocityToPlayer(-direction * hitStrenght);
                //}
            //}
        }

        
    } 
}
