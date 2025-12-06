using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class PlayerCheckLight : MonoBehaviour
{
    public static bool IsPlayerInLight(Transform player, Light2D light2D, LayerMask obstacleLayer)
    {
        Collider2D[] colliders = player.GetComponentsInChildren<Collider2D>();

        foreach (var col in colliders)
        {
            foreach (var point in GetColliderPoints(col))
            {
                Vector2 dir = (Vector2)light2D.transform.position - point;
                float dist = dir.magnitude;

                // Raycast từ point về phía light
                RaycastHit2D hit = Physics2D.Raycast(point, dir.normalized, dist, obstacleLayer);

                // Nếu không có vật cản → điểm này nhận được ánh sáng
                if (hit.collider == null)
                {
                    // Kiểm tra xem nằm trong radius ánh sáng
                    if (dist <= light2D.pointLightOuterRadius)
                        return true;
                }
            }
        }

        return false;
    }

    // Lấy tất cả điểm kiểm tra trên collider
    private static IEnumerable<Vector2> GetColliderPoints(Collider2D col)
    {
        // CircleCollider2D
        if (col is CircleCollider2D circle)
        {
            int segments = 12;
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                yield return (Vector2)circle.transform.TransformPoint(
                    circle.offset + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circle.radius
                );
            }
        }
        // BoxCollider2D
        else if (col is BoxCollider2D box)
        {
            Vector2 size = box.size * 0.5f;
            Vector2[] localPoints = new Vector2[]
            {
                new Vector2(-size.x, -size.y),
                new Vector2(-size.x,  size.y),
                new Vector2( size.x,  size.y),
                new Vector2( size.x, -size.y),
            };

            foreach (var p in localPoints)
                yield return (Vector2)box.transform.TransformPoint(box.offset + p);
        }
        // PolygonCollider2D
        else if (col is PolygonCollider2D poly)
        {
            for (int i = 0; i < poly.points.Length; i++)
                yield return poly.transform.TransformPoint(poly.points[i]);
        }
        // ✅ TilemapCollider2D
        else if (col is TilemapCollider2D tileCol)
        {
            Bounds b = tileCol.bounds;
            yield return b.center;
        }
        // ✅ CompositeCollider2D (PHẦN BẠN YÊU CẦU THÊM)
        else if (col is CompositeCollider2D composite)
        {
            int pathCount = composite.pathCount;
            Vector2[] pointsBuffer = new Vector2[64];

            for (int p = 0; p < pathCount; p++)
            {
                int pointCount = composite.GetPathPointCount(p);

                // Resize buffer nếu thiếu
                if (pointsBuffer.Length < pointCount)
                    pointsBuffer = new Vector2[pointCount];

                composite.GetPath(p, pointsBuffer);

                for (int i = 0; i < pointCount; i++)
                {
                    yield return composite.transform.TransformPoint(pointsBuffer[i]);
                }
            }
        }
    }

    public Transform player;
    public Light2D lightSource;
    public LayerMask obstacleMask;
    public bool inLight;

    void Update()
    {
        inLight = IsPlayerInLight(player, lightSource, obstacleMask);
    }
}
