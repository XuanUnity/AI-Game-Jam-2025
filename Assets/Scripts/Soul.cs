using System.Collections;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [Header("Float Effect")]
    public float floatAmplitude = 0.1f;   // Biên độ lơ lửng
    public float floatSpeed = 2f;          // Tốc độ lơ lửng

    [Header("Collect Effect")]
    public float collectDuration = 0.5f;   // Thời gian bay về player
    public float minScale = 0.3f;          // Scale nhỏ nhất khi biến mất

    private Vector3 startPos;
    private bool isCollected = false;
    private Coroutine floatRoutine;

    private void OnEnable()
    {
        // Reset trạng thái an toàn khi object được bật lại
        isCollected = false;
        startPos = transform.position;

        // Khởi động hiệu ứng lơ lửng
        if (floatRoutine != null)
            StopCoroutine(floatRoutine);

        floatRoutine = StartCoroutine(FloatEffect());
    }

    private void OnDisable()
    {
        // Tránh lỗi coroutine khi object bị tắt
        if (floatRoutine != null)
            StopCoroutine(floatRoutine);
    }

    private IEnumerator FloatEffect()
    {
        float time = 0f;

        while (!isCollected)
        {
            time += Time.deltaTime;

            float offsetY = Mathf.Sin(time * floatSpeed) * floatAmplitude;
            transform.position = startPos + new Vector3(0, offsetY, 0);

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Player"))
        {
            isCollected = true;

            // Dừng hiệu ứng lơ lửng
            if (floatRoutine != null)
                StopCoroutine(floatRoutine);

            StartCoroutine(MoveToPlayerAndDisappear(collision.transform));
        }
    }

    private IEnumerator MoveToPlayerAndDisappear(Transform player)
    {
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        float time = 0f;

        while (time < collectDuration)
        {
            if (player == null)
                break;

            time += Time.deltaTime;
            float t = time / collectDuration;

            // Bay về player
            transform.position = Vector3.Lerp(startPos, player.position, t);

            // Thu nhỏ dần
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * minScale, t);

            yield return null;
        }

        // Xóa an toàn
        Destroy(gameObject);
    }
}
