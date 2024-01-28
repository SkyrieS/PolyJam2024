using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayersController : MonoBehaviour
    {
        [SerializeField] private Player firstPlayer;
        [SerializeField] private Player secondPlayer;

        [SerializeField] private PlayerInputManager inputManager;

        private void Awake()
        {
            InitializeInputs();
            DisableInput();
        }

        public void EnableInput()
        {
            inputManager.ToggleInputs(true);
        }

        public void DisableInput()
        {
            inputManager.ToggleInputs(false);
        }

        private void InitializeInputs()
        {
            InputEvents firstPlayerEvents = new InputEvents(
                firstPlayer.PlayerLeftHandMovement.UpdateMovement,
                firstPlayer.PlayerRightHandMovement.UpdateMovement,
                firstPlayer.CloseLeftHand,
                firstPlayer.CloseRightHand
                );

            InputEvents secondPlayerEvents = new InputEvents(
                secondPlayer.PlayerLeftHandMovement.UpdateMovement,
                secondPlayer.PlayerRightHandMovement.UpdateMovement,
                secondPlayer.CloseLeftHand,
                secondPlayer.CloseRightHand
                );

            inputManager.InitializeInputs(firstPlayerEvents, secondPlayerEvents);
        }
    } 
}
