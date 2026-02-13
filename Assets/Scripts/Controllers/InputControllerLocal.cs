using System;
using UnityEngine;

namespace MultiplayerBlob.Controllers
{ 
    public class InputControllerLocal : InputControllerAbstract
    {
        // Cached References
        private InputSystem_Actions playerInput;
        
        // Events
        public override event Action<MovementContext> movementPerformed;
        
        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();
            playerInput = new InputSystem_Actions();
            playerInput.Player.Move.performed += context => ParseMovement(context.ReadValue<Vector2>());
            playerInput.Player.Move.canceled += _ => ParseMovement(Vector2.zero);
            
            playerInput.Player.Jump.performed += _ => ParseJump();
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }

        private void OnDisable()
        {
            playerInput.Disable();
        }
        #endregion

        #region InputHandlers
        private void ParseMovement(Vector2 readValue)
        {
            movementPerformed?.Invoke(new MovementContext(MovementType.Move, readValue));
        }

        private void ParseJump()
        {
            movementPerformed?.Invoke(new MovementContext(MovementType.Jump));
        }
        #endregion
    }
}
