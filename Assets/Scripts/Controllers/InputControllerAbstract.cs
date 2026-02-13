using System;
using UnityEngine;

namespace MultiplayerBlob.Controllers
{
    public abstract class InputControllerAbstract : MonoBehaviour
    {
        public abstract event Action<MovementContext> movementPerformed;

        protected virtual void Awake()
        {
            VerifyUnique();
        }
        
        private void VerifyUnique()
        {
            var playerControllers = FindObjectsByType<InputControllerAbstract>(FindObjectsSortMode.None);
            if (playerControllers.Length > 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
