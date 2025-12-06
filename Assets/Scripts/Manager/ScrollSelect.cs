using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSelect : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float leftLimit = -500f;
    [SerializeField] private float rightLimit = 500f;

    private Vector3 lastMousePos;
    private float velocityX;

    void Update()
    {
        HandleScroll();
        ApplyInertia();
        ClampPosition();
    }

    void HandleScroll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            velocityX = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float moveX = delta.x * scrollSpeed * Time.deltaTime;

            transform.position += new Vector3(moveX, 0, 0);

            velocityX = moveX * 60f; // Lưu tốc độ theo frame
            lastMousePos = Input.mousePosition;
        }
    }

    void ApplyInertia()
    {
        if (!Input.GetMouseButton(0))
        {
            if (Mathf.Abs(velocityX) > 0.01f)
            {
                transform.position += new Vector3(velocityX * Time.deltaTime, 0, 0);
                velocityX = Mathf.Lerp(velocityX, 0, smoothSpeed * Time.deltaTime);
            }
        }
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        transform.position = pos;
    }
}
