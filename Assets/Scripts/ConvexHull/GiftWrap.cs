using System.Collections.Generic;
using UnityEngine;

namespace ConvexHull
{
    public static class GiftWrap
    {
        public static List<Vector3> GetConvexHull(IList<Vector3> points)
        {
            int n = points.Count;
            if (n < 3) return new List<Vector3>();
            
            int leftmost = 0;
            for (int i = 1; i < n; i++)
            {
                if (points[i].x < points[leftmost].x) { leftmost = i; }
            }

            var hull = new List<Vector3>();
            int p = leftmost;
            do
            {
                hull.Add(points[p]);
                var q = (p + 1) % n;
                for (int i = 0; i < n; i++)
                {
                    if (GetGiftWrapOrientation(points[p], points[i], points[q]) == GiftWrapOrientation.CounterClockwise)
                    {
                        q = i;
                    }
                }
                p = q;
            } while (p != leftmost);

            return hull;
        }
        
        private static GiftWrapOrientation GetGiftWrapOrientation(Vector3 p, Vector3 q, Vector3 r)
        {
            return ((q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y)) switch
            {
                > 0 => GiftWrapOrientation.Clockwise,
                < 0 => GiftWrapOrientation.CounterClockwise,
                _ => GiftWrapOrientation.Collinear
            };
        }
    }
}
