using Blob;
using MultiplayerBlob.Controllers;
using UnityEngine;

namespace MultiplayerBlob.Blob
{
    [RequireComponent(typeof(InputControllerAbstract))]
    public class BlobAgentMoverExtrinsic : BlobAgentMoverAbstract
    {
        // State
        private float xLookDirection;
        private float yLookDirection;
        
        // Cached References
        private InputControllerAbstract inputControllerAbstract;
        
        #region Static
        private const float _signFloorThreshold = 0.1f;
        private static float SignFloored(float number) => Mathf.Abs(number) < _signFloorThreshold ? 0 : Mathf.Sign(number);
        #endregion
        
        #region UnityMethods
        private void Awake()
        {
            inputControllerAbstract = GetComponent<InputControllerAbstract>();
        }

        private void Update()
        {
            if (!isInitialized) { return; }
            Move();
        }

        private void OnEnable()
        {
            inputControllerAbstract.movementPerformed += HandleMovement;

        }

        private void OnDisable()
        {
            inputControllerAbstract.movementPerformed += HandleMovement;
        }
        #endregion
        
        #region PrivateMethods
        private void HandleMovement(MovementContext movementContext)
        {
            if (!isInitialized) { return; }
            
            switch (movementContext.movementType)
            {
                case MovementType.Move:
                    SetMoveParameters(movementContext.movementDirection);
                    break;
                case MovementType.Jump:
                    Jump();
                    break;
            }
        }

        private void SetMoveParameters(Vector2 movementDirection)
        {
            xLookDirection = SignFloored(movementDirection.x);
            yLookDirection = SignFloored(movementDirection.y);
        }

        private void Move()
        {
            if (Mathf.Approximately(xLookDirection, 0f) && Mathf.Approximately(yLookDirection, 0f)) { return; }

            var newLocalPosition = new Vector2(
                transform.localPosition.x + xLookDirection * movementSpeed * Time.deltaTime,
                transform.localPosition.y + yLookDirection * movementSpeed * Time.deltaTime);
            if (DoesExceedExtents(newLocalPosition)) { return; }
            
            transform.localPosition = newLocalPosition;
        }

        private void Jump()
        {
            
        }
        #endregion
    }
}