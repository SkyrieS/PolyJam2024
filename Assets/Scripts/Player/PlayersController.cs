using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayersController : MonoBehaviour
    {
        [SerializeField] private Player firstPlayer;
        [SerializeField] private Player secondPlayer;

        private InputActions _inputActions;

        private void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            EnableInputs();
        }

        private void OnDisable()
        {
            DisableInputs();
        }

        private void EnableInputs()
        {
            _inputActions.Game.MovementPlayer1.performed += firstPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer1.canceled += firstPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer1.Enable();

            _inputActions.Game.PickLeftHandPlayer1.performed += firstPlayer.PickOrDropItemLeftHand;
            _inputActions.Game.PickLeftHandPlayer1.Enable();

            _inputActions.Game.PickRightHandPlayer1.performed += firstPlayer.PickOrDropItemRightHand;
            _inputActions.Game.PickRightHandPlayer1.Enable();

            _inputActions.Game.MovementPlayer2.performed += secondPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer2.canceled += secondPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer2.Enable();

            _inputActions.Game.PickLeftHandPlayer2.performed += secondPlayer.PickOrDropItemLeftHand;
            _inputActions.Game.PickLeftHandPlayer2.Enable();

            _inputActions.Game.PickRightHandPlayer2.performed += secondPlayer.PickOrDropItemRightHand;
            _inputActions.Game.PickRightHandPlayer2.Enable();
        }

        private void DisableInputs()
        {
            _inputActions.Game.MovementPlayer1.performed -= firstPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer1.canceled -= firstPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer1.Disable();

            _inputActions.Game.PickLeftHandPlayer1.performed -= firstPlayer.PickOrDropItemLeftHand;
            _inputActions.Game.PickLeftHandPlayer1.Disable();

            _inputActions.Game.PickRightHandPlayer1.performed -= firstPlayer.PickOrDropItemRightHand;
            _inputActions.Game.PickRightHandPlayer1.Disable();

            _inputActions.Game.MovementPlayer2.performed -= secondPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer2.canceled -= secondPlayer.PlayerMovement.UpdateMovement;
            _inputActions.Game.MovementPlayer2.Disable();


            _inputActions.Game.PickLeftHandPlayer2.performed -= secondPlayer.PickOrDropItemLeftHand;
            _inputActions.Game.PickLeftHandPlayer2.Disable();

            _inputActions.Game.PickRightHandPlayer2.performed -= secondPlayer.PickOrDropItemRightHand;
            _inputActions.Game.PickRightHandPlayer2.Disable();
        }
    } 
}
