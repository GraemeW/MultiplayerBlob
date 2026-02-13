using System.Collections.Generic;
using MultiplayerBlob;
using MultiplayerBlob.Blob;
using UnityEngine;

namespace Blob
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BlobMover : MonoBehaviour
    {
        // Tunables
        [Header("Blob Properties")]
        [SerializeField] private float blobMoveSpeed = 50f;
        [Header("Agent Properties")]
        [SerializeField] private float extrinsicAgentExtents = 25f;
        [SerializeField] private float extrinsicAgentMoveSpeed = 100f;
        [SerializeField] private float intrinsicAgentExtents = 15f;
        [SerializeField] private float intrinsicAgentMovementSpeed = 30f;
        
        // State
        private readonly List<BlobAgentMoverExtrinsic> blobAgents = new();
        
        // Cached References
        private Rigidbody2D rigidBody2D;
        
        #region UnityMethods
        private void Awake()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            InitializeIntrinsicAgents();
            InitializeExtrinsicAgents();
        }

        private void FixedUpdate()
        {
            Vector2 newPosition = GetNewPositionFromExtrinsicAgents();
            if (newPosition == Vector2.zero) { return; }
            
            var deltaPosition = new Vector2(newPosition.x - transform.position.x, newPosition.y - transform.position.y);
            rigidBody2D.MovePosition(newPosition);
            ReconcileExtrinsicAgentPositions(deltaPosition);
        }

        #endregion
        
        #region PublicMethods
        public void RegisterBlobAgent(BlobAgentMoverExtrinsic blobAgent)
        {
            if (blobAgent == null) { return;  }
            blobAgents.Add(blobAgent);
        }

        public void UnregisterBlobAgent(BlobAgentMoverExtrinsic blobAgent)
        {
            if (blobAgent == null) { return; }
            blobAgents.Remove(blobAgent);
        }
        #endregion
        
        #region PrivateMethods

        private void InitializeIntrinsicAgents()
        {
            foreach (BlobAgentMoverIntrinsic intrinsicAgent in GetComponentsInChildren<BlobAgentMoverIntrinsic>())
            {
                intrinsicAgent.Initialize(intrinsicAgentExtents, intrinsicAgentMovementSpeed);
            }
        }

        private void InitializeExtrinsicAgents()
        {
            foreach (BlobAgentMoverExtrinsic extrinsicAgent in GetComponentsInChildren<BlobAgentMoverExtrinsic>())
            {
                extrinsicAgent.Initialize(extrinsicAgentExtents, extrinsicAgentMoveSpeed);
                RegisterBlobAgent(extrinsicAgent);
            }
        }
        
        private Vector2 GetNewPositionFromExtrinsicAgents()
        {
            float xPositionTarget = 0f;
            float yPositionTarget = 0f;
            foreach (BlobAgentMoverExtrinsic agent in blobAgents)
            {
                if (agent == null) { UnregisterBlobAgent(agent); continue;}
                xPositionTarget += agent.transform.position.x;
                yPositionTarget += agent.transform.position.y;
            }
            if (blobAgents.Count == 0) { return Vector2.zero; }
            
            xPositionTarget /= blobAgents.Count;
            yPositionTarget /= blobAgents.Count;
            return Vector2.MoveTowards(transform.position, new Vector2(xPositionTarget, yPositionTarget), blobMoveSpeed * Time.deltaTime);
        }

        private void ReconcileExtrinsicAgentPositions(Vector2 deltaPosition)
        {
            foreach (BlobAgentMoverExtrinsic agent in blobAgents)
            {
                agent.transform.localPosition =  new Vector3(
                    agent.transform.localPosition.x - deltaPosition.x, 
                    agent.transform.localPosition.y - deltaPosition.y, 
                    agent.transform.localPosition.z);
            }
        }
        #endregion
    }
}
