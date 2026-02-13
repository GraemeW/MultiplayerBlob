using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ConvexHull;

namespace MultiplayerBlob
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BlobMeshGenerator : MonoBehaviour
    {
        // Tunables
        [SerializeField] private bool useConvexHull;
        
        // Cached References
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        #region StaticMethods
        private const string _blobSortingLayerRef = "Blob";

        private static List<List<int>> GenerateTriangles(IList<int> pointIndices)
        {
            List<List<int>> combinations = new();
            if (pointIndices.Count < 3) { return combinations; }

            for (int i = 0; i <= pointIndices.Count - 3; i++)
            {
                for (int j = i + 1; j <= pointIndices.Count - 3 + 1; j++)
                {
                    for (int k = j + 1; k <= pointIndices.Count - 3 + 2; k++)
                    {
                        combinations.Add(new List<int>{ pointIndices[i], pointIndices[j], pointIndices[k] });
                    }
                }
            }
            return combinations;
        }
        #endregion
        
        #region UnityMethods
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            meshRenderer.sortingLayerName = _blobSortingLayerRef;
            meshRenderer.sortingOrder = 0;
        }

        private void Update()
        {
            GenerateBlobMesh();
        }
        #endregion

        private void GenerateBlobMesh()
        {
            var mesh = new Mesh();
            Vector3[] points = (from Transform child in transform select child.localPosition).ToArray();
            if (points.Length < 3)
            {
                Debug.Log("Insufficient points to draw blob");
                return;
            }

            if (useConvexHull) { points = GiftWrap.GetConvexHull(points).ToArray(); }

            mesh.vertices = points;
            mesh.triangles = GenerateTriangles(Enumerable.Range(0, points.Length).ToList()).SelectMany(triangleIndices => triangleIndices).ToArray();
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
        }
    }
}
