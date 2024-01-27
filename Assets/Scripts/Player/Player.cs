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
        [SerializeField] private PlayerType player;
        public PlayerType Type => player;

        [SerializeField] private PlayerHandMovement playerLeftHandMovement;
        [SerializeField] private PlayerHandMovement playerRightHandMovement;

        [SerializeField] private PlayerClawMover playerLeftClawMover;
        [SerializeField] private PlayerClawMover playerRightClawMover;

        public PlayerHandMovement PlayerLeftHandMovement { get { return playerLeftHandMovement; } }
        public PlayerHandMovement PlayerRightHandMovement { get { return playerRightHandMovement; } }

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

        public void CloseLeftHand(bool isClosing)
        {
            playerLeftClawMover.ToggleClosing(isClosing);
        }

        public void CloseRightHand(bool isClosing)
        {
            playerRightClawMover.ToggleClosing(isClosing);
        }

        public void AddVelocityToPlayer(Vector2 direction)
        {
            //playerMovement.AddVelocity(direction);
        }

        public void AddItemToHand(IPickable item, HandType handType)
        {
            if (handType.Equals(HandType.Left) && leftHandItem == null)
            {
                leftHandItem = item;
                item.Object.transform.localPosition = Vector3.zero;
                item.Object.transform.localRotation = Quaternion.identity;
            }

            if (handType.Equals(HandType.Right) && rightHandItem == null)
            {
                rightHandItem = item;
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
        }

        private void DropItem(HandType handType)
        {
            if (handType.Equals(HandType.Left))
            {
                leftHandItem.Object.transform.parent = null;
                leftHandItem.DropItem();
                AddPlayerVelocityToItem(leftHandItem);
                leftHandItem = null;
            }
            else if (handType.Equals(HandType.Right))
            {
                rightHandItem.Object.transform.parent = null;
                rightHandItem.DropItem();
                AddPlayerVelocityToItem(rightHandItem);
                rightHandItem = null;
            }
        }

        private void AddPlayerVelocityToItem(IPickable item)
        {
            //item.Rigidbody2D.velocity = playerMovement.PlayerRigidbody2D.velocity;
        }
    }
}