using UnityEngine;

namespace MultiplayerBlob
{
    public class BlobAgentMoverIntrinsic : MonoBehaviour
    {
        // Tunables
        [SerializeField][Tooltip("how far the agent will travel")] private float traverseExtents = 15f;
        [SerializeField][Tooltip("the time for the blob to traverse extents")] private float periodSeconds = 0.5f;

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
            bool shouldMoveTowardCentre = DidExceedExtents();
            UpdateMovementAngle(shouldMoveTowardCentre);
            UpdateIncrements();
            ShiftAgentPosition();
        }
        #endregion

        #region PrivateMethods
        private void UpdateMovementAngle(bool forceTowardCentre)
        {
            angleAdjustTimer += Time.deltaTime;

            if (forceTowardCentre)
            {
                orientationAngle = Mathf.Atan2(-transform.localPosition.y, -transform.localPosition.x);
            }
            else
            {
                if (angleAdjustTimer < periodSeconds) { return; }
                orientationAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                angleAdjustTimer = 0;
            }
        }
        
        private void UpdateIncrements()
        {
            xIncrement = Mathf.Cos(orientationAngle) * traverseExtents / periodSeconds;
            yIncrement = Mathf.Sin(orientationAngle) * traverseExtents /  periodSeconds;
        }

        private void ShiftAgentPosition()
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x + xIncrement * Time.deltaTime,
                transform.localPosition.y + yIncrement * Time.deltaTime,
                transform.localPosition.z);
        }
        
        private bool DidExceedExtents()
        {
            return Mathf.Pow(transform.localPosition.x, 2) + Mathf.Pow(transform.localPosition.y, 2) > Mathf.Pow(traverseExtents,2);
        }
        #endregion
    }
}
