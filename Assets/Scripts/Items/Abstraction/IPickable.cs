using Game.Player;
using UnityEngine;

namespace Game.Items
{
	public interface IPickable
	{
        IPlayer ParentPlayer { get; }
        GameObject Object { get; }
		Rigidbody2D Rigidbody2D { get; }
        bool IsHeld { get; }
		void PickItem(IPlayer player, HandType handType);
		void DropItem();
	} 
}