using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayer
    {
        void AddVelocityToPlayer(Vector2 direction);
        void AddItemToHand(IPickable item, HandType handType);
    }

    public enum HandType
    {
        Left,
        Right
    } 
}