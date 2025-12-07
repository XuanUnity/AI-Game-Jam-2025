using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class PlayerCheckLight : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Light2D lightSource;
    public LayerMask obstacleMask;

    [Header("Debug")]
    public bool debugRays = false;

    [Header("Quality Settings")]
    public int circleSamples = 12;
    public int tilemapGridSamples = 3;

    [HideInInspector]
    public bool inLight;

    // =================== API CỦA BẠN (GIỮ NGUYÊN) ===================
    public void SetLight2D(GameObject map)
    {
        if (map == null)
        {
            Debug.LogError("Map is null!");
            lightSource = null;
            return;
        }

        // Tìm object con tên "Light"
        Transform lightParent = map.transform.Find("Light");
        if (lightParent == null)
        {
            Debug.LogError("Không tìm thấy object 'Light' trong Map!");
            lightSource = null;
            return;
        }

        // Tìm Light2D
        Light2D light2D = lightParent.GetComponentInChildren<Light2D>(true);
        if (light2D == null)
        {
            Debug.LogError("Không tìm thấy Light2D trong 'Light'!");
            lightSource = null;
            return;
        }

        lightSource = light2D;
    }

    void Update()
    {
        inLight = IsPlayerInLight(player, lightSource, obstacleMask);
    }

    // =================== CORE LOGIC (ĐÃ FIX) ===================
    public static bool IsPlayerInLight(Transform player, Light2D light2D, LayerMask obstacleLayer)
    {
        if (player == null || light2D == null)
            return false;

        Collider2D[] colliders = player.GetComponentsInChildren<Collider2D>();
        Vector2 lightPos = (Vector2)light2D.transform.position;

        foreach (var col in colliders)
        {
            if (col == null) continue;

            foreach (var point in GetColliderPoints(col))
            {
                // Check distance trước
                float dist = Vector2.Distance(lightPos, point);
                if (dist > light2D.pointLightOuterRadius)
                    continue;

                Vector2 dir = point - lightPos;
                float rayDist = dir.magnitude;

                if (rayDist <= 0.0001f)
                    return true;

                // ✅ Raycast từ LIGHT -> PLAYER
                RaycastHit2D hit = Physics2D.Raycast(lightPos, dir.normalized, rayDist, obstacleLayer);

                if (hit.collider == null)
                {
                    return true; // Không bị che → có ánh sáng
                }
            }
        }

        return false;
    }

    // =================== COLLIDER SAMPLING (ĐÃ FIX) ===================
    private static IEnumerable<Vector2> GetColliderPoints(Collider2D col)
    {
        // CircleCollider2D
        if (col is CircleCollider2D circle)
        {
            int segments = 12;
            yield return (Vector2)circle.transform.TransformPoint(circle.offset);

            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                Vector2 local = circle.offset + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circle.radius;
                yield return (Vector2)circle.transform.TransformPoint(local);
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
                Vector2.zero
            };

            foreach (var p in localPoints)
                yield return (Vector2)box.transform.TransformPoint(box.offset + p);
        }
        // PolygonCollider2D
        else if (col is PolygonCollider2D poly)
        {
            Vector2 center = Vector2.zero;
            for (int i = 0; i < poly.points.Length; i++)
            {
                center += poly.points[i];
                yield return poly.transform.TransformPoint(poly.points[i]);
            }

            if (poly.points.Length > 0)
            {
                center /= poly.points.Length;
                yield return poly.transform.TransformPoint(center);
            }
        }
        // ✅ TilemapCollider2D
        else if (col is TilemapCollider2D tileCol)
        {
            if (tileCol.usedByComposite)
                yield break;

            Bounds b = tileCol.bounds;
            int grid = 3;

            for (int y = 0; y < grid; y++)
            {
                float ty = Mathf.Lerp(b.min.y, b.max.y, (grid == 1) ? 0.5f : (float)y / (grid - 1));
                for (int x = 0; x < grid; x++)
                {
                    float tx = Mathf.Lerp(b.min.x, b.max.x, (grid == 1) ? 0.5f : (float)x / (grid - 1));
                    yield return new Vector2(tx, ty);
                }
            }
        }
        // ✅ CompositeCollider2D
        else if (col is CompositeCollider2D composite)
        {
            int pathCount = composite.pathCount;
            Vector2[] buffer = new Vector2[64];

            for (int p = 0; p < pathCount; p++)
            {
                int pointCount = composite.GetPathPointCount(p);
                if (pointCount <= 0) continue;

                if (buffer.Length < pointCount)
                    buffer = new Vector2[pointCount];

                composite.GetPath(p, buffer);

                // center
                Vector2 center = Vector2.zero;
                for (int i = 0; i < pointCount; i++)
                    center += buffer[i];
                center /= pointCount;

                yield return composite.transform.TransformPoint(center);

                // all vertices
                for (int i = 0; i < pointCount; i++)
                    yield return composite.transform.TransformPoint(buffer[i]);
            }
        }
        // Fallback
        else
        {
            yield return col.transform.position;
        }
    }
}
