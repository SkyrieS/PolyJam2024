using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayer
    {
        PlayerType Type { get; }
    }

    public enum PlayerType
    {
        None,
        PlayerOne,
        PlayerTwo
    }

    public enum HandType
    {
        Left,
        Right
    } 
}