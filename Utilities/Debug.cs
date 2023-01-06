using System.Linq;
using UnityEngine;

namespace NorskaLib.Utilities
{
    public struct DebugUtils
    {
        public static void DrawPolyline(Vector3[] vertices, bool loop, Color color, float duration = 1)
        {
            if (loop)
                Debug.DrawLine(vertices.Last(), vertices.First(), color, duration);

            for (int i = 1; i < vertices.Length; i++)
                Debug.DrawLine(vertices[i - 1], vertices[i], color, duration);
        }

        public static void DrawSector(Vector3 origin, float facing, float span, float radius, Color color, float duration = 1, int subdivision = 2)
        {
            //var vertices = new List<Vector3>();
            //var angularOrigin = facing - (span * 0.5f);
            //var angularLimit = facing + (span * 0.5f);

            //subdivision = subdivision < 2 ? 2 : subdivision;
            //var arcSubposCount = subdivision - 1;
            //var arcSubposDelta = 1.0f / subdivision;

            //// Origin
            //vertices.Add(origin);
            //vertices.Add(MathUtils.PositionOnCircle3D(origin, angularOrigin, radius));
            //for (int i = 1; i <= arcSubposCount; i++)
            //{
            //    var angle = Mathf.Lerp(angularOrigin, angularLimit, arcSubposDelta * i);
            //    vertices.Add(MathUtils.PositionOnCircle3D(origin, angle, radius));
            //}
            //vertices.Add(MathUtils.PositionOnCircle3D(origin, angularLimit, radius));

            var vertices = MeshUtils.GetSectorVertices(origin, facing, span, radius, subdivision);
            DrawPolyline(vertices, true, color, duration);
        }

        public static void DrawSector(Vector3 origin, float facing, float span, float radiusMin, float radiusMax, Color color, float duration = 1, int subdivision = 2)
        {
            //subdivision = subdivision < 2 ? 2 : subdivision;
            //var arcSubposDelta = 1.0f / subdivision;
            //var angularOrigin = facing - (span * 0.5f);
            //var angularLimit = facing + (span * 0.5f);

            //var vertices = new List<Vector3>();
            //// Top-Left
            //vertices.Add(MathUtils.PositionOnCircle3D(origin, angularOrigin, radiusMax));
            //for (int i = 1; i < subdivision; i++)
            //{
            //    var angle = Mathf.Lerp(angularOrigin, angularLimit, arcSubposDelta * i);
            //    vertices.Add(MathUtils.PositionOnCircle3D(origin, angle, radiusMax));
            //}
            //// Top-Right
            //vertices.Add(MathUtils.PositionOnCircle3D(origin, angularLimit, radiusMax));

            //// Bottom-Right
            //vertices.Add(MathUtils.PositionOnCircle3D(origin, angularLimit, radiusMin));
            //for (int i = subdivision - 1; i > 0; i--)
            //{
            //    var angle = Mathf.Lerp(angularOrigin, angularLimit, arcSubposDelta * i);
            //    vertices.Add(MathUtils.PositionOnCircle3D(origin, angle, radiusMin));
            //}

            ////Bottom-Left
            //vertices.Add(MathUtils.PositionOnCircle3D(origin, angularOrigin, radiusMin));

            var vertices = MeshUtils.GetSectorVertices(origin, facing, span, radiusMin, radiusMax, subdivision);
            DrawPolyline(vertices, true, color, duration);
        }

        public static void DrawCircle(Vector3 origin, float radius, Color color, float duration = 1, int subdivision = 8)
        {
            var vertices = MeshUtils.GetCircleVertices(origin, radius, subdivision);
            DrawPolyline(vertices, true, color, duration);
        }
    }
}
