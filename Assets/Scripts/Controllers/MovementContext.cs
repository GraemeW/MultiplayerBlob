using UnityEngine;

namespace MultiplayerBlob.Controllers
{
    public struct MovementContext
    {
        public readonly MovementType movementType;
        public readonly Vector2 movementDirection;

        public MovementContext(MovementType movementType, Vector2 movementDirection)
        {
            this.movementType = movementType;
            this.movementDirection = movementDirection;
        }

        public MovementContext(MovementType movementType)
        {
            this.movementType = movementType;
            movementDirection = Vector2.zero;
        }
    }
}
