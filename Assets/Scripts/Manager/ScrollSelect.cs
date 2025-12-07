using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSelect : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;

    private RectTransform rect;
    private Vector2 lastMousePos;
    private float velocityX;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (TutorialManager.Instance.isTutorial) return;

        HandleScroll();
        ApplyInertia();
        ClampPosition();
    }

    void HandleScroll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            velocityX = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.mousePosition.x - lastMousePos.x;
            float move = deltaX * scrollSpeed * Time.deltaTime;

            rect.anchoredPosition += new Vector2(move, 0);
            velocityX = move / Time.deltaTime;

            lastMousePos = Input.mousePosition;
        }
    }

    void ApplyInertia()
    {
        if (!Input.GetMouseButton(0))
        {
            velocityX = Mathf.Lerp(velocityX, 0f, smoothSpeed * Time.deltaTime);
            rect.anchoredPosition += new Vector2(velocityX * Time.deltaTime, 0);
        }
    }

    void ClampPosition()
    {
        Vector2 pos = rect.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        rect.anchoredPosition = pos;
    }
}
