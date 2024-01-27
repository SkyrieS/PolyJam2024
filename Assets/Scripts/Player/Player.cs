using Game.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

namespace Game.Player
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Transform leftItemParent;
        [SerializeField] private Rigidbody2D leftHandRb;
        [SerializeField] private Transform rightItemParent;
        [SerializeField] private Rigidbody2D rightHandRb;
        [SerializeField] private float freeHandAngularDrag = 1f;
        [SerializeField] private float fullHandAngularDrag = 5f;
        public PlayerMovement PlayerMovement { get { return playerMovement; } }

        private List<IPickable> itemsToPickup;

        private IPickable leftHandItem;
        private IPickable rightHandItem;

        private void Start()
        {
            itemsToPickup = new List<IPickable>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IPickable item))
            {
                if(!item.IsHeld && !itemsToPickup.Contains(item))
                {
                    itemsToPickup.Add(item);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IPickable item))
            {
                if (!item.IsHeld && itemsToPickup.Contains(item))
                {
                    itemsToPickup.Remove(item);
                }
            }
        }

        public void AddItemToHand(IPickable item, HandType handType)
        {
            if (handType.Equals(HandType.Left) && leftHandItem == null)
            {
                leftHandItem = item;
                item.Object.transform.parent = leftItemParent;
                item.Object.transform.localPosition = Vector3.zero;
                item.Object.transform.localRotation = Quaternion.identity;
            }

            if (handType.Equals(HandType.Right) && rightHandItem == null)
            {
                rightHandItem = item;
                item.Object.transform.parent = rightItemParent;
                item.Object.transform.localPosition = Vector3.zero;
                item.Object.transform.localRotation = Quaternion.identity;
            }
        }

        public void PickOrDropItemLeftHand(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                if (leftHandItem == null)
                {
                    if (itemsToPickup.Count != 0)
                        PickItem(HandType.Left);
                }
                else
                {
                    DropItem(HandType.Left);
                }
            }
        }

        public void PickOrDropItemRightHand(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                if (rightHandItem == null)
                {
                    if (itemsToPickup.Count != 0)
                        PickItem(HandType.Right);
                }
                else
                {
                    DropItem(HandType.Right);
                }
            }
        }

        private void PickItem(HandType handType)
        {
            var firstItem = itemsToPickup.First();
            itemsToPickup.Remove(firstItem);
            firstItem.PickItem(this, handType);

            if (handType.Equals(HandType.Left))
            {
                leftHandRb.angularDrag = fullHandAngularDrag;
            }
            else if (handType.Equals(HandType.Right))
            {
                rightHandRb.angularDrag = fullHandAngularDrag;
            }
        }

        private void DropItem(HandType handType)
        {
            if (handType.Equals(HandType.Left))
            {
                leftHandItem.Object.transform.parent = null;
                leftHandItem.DropItem();
                AddPlayerVelocityToItem(leftHandItem);
                leftHandItem = null;
                leftHandRb.angularDrag = freeHandAngularDrag;
            }
            else if (handType.Equals(HandType.Right))
            {
                rightHandItem.Object.transform.parent = null;
                rightHandItem.DropItem();
                AddPlayerVelocityToItem(rightHandItem);
                rightHandItem = null;
                rightHandRb.angularDrag = freeHandAngularDrag;
            }
        }

        private void AddPlayerVelocityToItem(IPickable item)
        {
            item.Rigidbody2D.velocity = playerMovement.PlayerRigidbody2D.velocity;
        }
    }
}