using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float flyDuration = 1.5f;       // Thời gian bay về cửa
    public float targetScale = 0.1f;       // Kích thước nhỏ lại

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(FlyAndShrinkPlayer(collision.transform));
            GameManagerInMap.Instance.PauseLight();

            if(TutorialManager.Instance.isTutorial)
            {
                TutorialManager.Instance.isTutorial = false;
            }
        }
    }

    private IEnumerator FlyAndShrinkPlayer(Transform player)
    {
        // Tắt điều khiển player (nếu có)
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        Vector3 startPos = player.position;
        Vector3 targetPos = transform.position;

        Vector3 startScale = player.localScale;
        Vector3 endScale = Vector3.one * targetScale;

        float time = 0f;

        while (time < flyDuration)
        {
            time += Time.deltaTime;
            float t = time / flyDuration;

            // Di chuyển mượt về tâm cửa
            player.position = Vector3.Lerp(startPos, targetPos, t);

            // Thu nhỏ dần
            player.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        // Đảm bảo vị trí và scale chính xác khi kết thúc
        player.position = targetPos;
        player.localScale = endScale;
        player.gameObject.SetActive(false);

        // Gọi win game
        GameManagerInMap.Instance.WinGame();
    }
}
