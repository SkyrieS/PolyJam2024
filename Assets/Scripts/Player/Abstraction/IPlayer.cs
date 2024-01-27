using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayer
    {
        void AddItemToHand(IPickable item, HandType handType);
    }

    public enum HandType
    {
        Left,
        Right
    } 
}