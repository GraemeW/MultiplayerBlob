using UnityEngine;

namespace Blob
{
    public class BlobAgentMoverAbstract : MonoBehaviour
    {
        // Configurable Parameters
        private float traverseExtentsSquared;
        protected float movementSpeed;
        
        // State
        protected bool isInitialized = false;
        
        public virtual void Initialize(float setTraverseExtents, float setMovementSpeed)
        {
            traverseExtentsSquared = Mathf.Pow(setTraverseExtents,2);
            movementSpeed = setMovementSpeed;
            isInitialized = true;
        }
        
        protected bool DoesExceedExtents(Vector2 position)
        {
            return Mathf.Pow(position.x, 2) + Mathf.Pow(position.y, 2) > traverseExtentsSquared;
        }
    }
}
