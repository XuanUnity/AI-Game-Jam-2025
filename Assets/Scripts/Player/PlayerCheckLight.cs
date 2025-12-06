using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
        else if (col is PolygonCollider2D poly)
        {
            for (int i = 0; i < poly.points.Length; i++)
                yield return poly.transform.TransformPoint(poly.points[i]);
        }
    }
    public Transform player;
    public Light2D lightSource;
    public LayerMask obstacleMask;
    public bool inLight;

    void Update()
    {
        inLight = IsPlayerInLight(player, lightSource, obstacleMask);

        if (inLight)
            Debug.Log("Player đang đứng trong ánh sáng!");
        else
            Debug.Log("Player đang ở trong bóng tối!");
    }
}
