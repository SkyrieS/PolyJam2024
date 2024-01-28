using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Items
{
	public interface IPickable
	{
		Transform transform { get; }
		FixedJoint2D Joint { get; }
		UnityAction OnGrabbed { get; set; }
		Rigidbody2D Rigidbody2D { get; }

        bool IsHeld { get; set; }
		void PickItem() { IsHeld = true; OnGrabbed?.Invoke(); }
		void DropItem() { IsHeld = false; }
	} 
}