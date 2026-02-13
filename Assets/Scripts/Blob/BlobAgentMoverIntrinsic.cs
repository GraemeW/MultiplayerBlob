using Blob;
using UnityEngine;

namespace MultiplayerBlob
{
    public class BlobAgentMoverIntrinsic : BlobAgentMoverAbstract
    {
        // Derived Parameters
        private float periodSeconds = 0.5f;

        // State
        private float angleAdjustTimer;
        private float orientationAngle;
        private float xIncrement;
        private float yIncrement;

        #region UnityMethods
        private void Awake()
        {
            orientationAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        }
        
        private void Update()
        {
            if (!isInitialized) { return; }
            bool shouldMoveTowardCentre = DoesExceedExtents(transform.localPosition);
            UpdateMovementAngle(shouldMoveTowardCentre);
            ShiftAgentPosition();
        }
        #endregion
        
        #region PublicProtectedMethods

        public override void Initialize(float setTraverseExtents, float setMovementSpeed)
        {
            base.Initialize(setTraverseExtents, setMovementSpeed);
            periodSeconds = setTraverseExtents / setMovementSpeed;
        }
        #endregion

        #region PrivateMethods
        private void UpdateMovementAngle(bool forceTowardCentre)
        {
            angleAdjustTimer += Time.deltaTime;

            if (forceTowardCentre)
            {
                orientationAngle = Mathf.Atan2(-transform.localPosition.y, -transform.localPosition.x);
                UpdateIncrements();
            }
            else
            {
                if (angleAdjustTimer < periodSeconds) { return; }
                orientationAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                UpdateIncrements();
                angleAdjustTimer = 0;
            }
        }
        
        private void UpdateIncrements()
        {
            xIncrement = Mathf.Cos(orientationAngle) * movementSpeed;
            yIncrement = Mathf.Sin(orientationAngle) * movementSpeed;
        }

        private void ShiftAgentPosition()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x + xIncrement * Time.deltaTime,
                transform.localPosition.y + yIncrement * Time.deltaTime,
                transform.localPosition.z);
        }
        #endregion
    }
}
