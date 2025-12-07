using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;
    public Image fillImage;

    [Header("Time")]
    public float duration; // Tổng thời gian

    [Header("Colors")]
    public Color startColor = Color.yellow;
    public Color endColor = Color.gray;

    private float timer = 0f;
    private bool isRunning = false;
    private bool isReversing = false;

    [SerializeField] private TextMeshProUGUI textTime;
    private float currentTime;
    private float maxtime;

    void Update()
    {
        if (!isRunning) return;
        if(isReversing)
        {
            timer -= Time.deltaTime * 2;
            currentTime += Time.deltaTime * 2;
            if(currentTime > maxtime)
            {
                currentTime = maxtime;
            }
        } else
        {
            timer += Time.deltaTime;
            currentTime -= Time.deltaTime;
            if(currentTime < 0f)
            {
                currentTime = 0f;
            }
        }

        float t = Mathf.Clamp01(timer / duration);

        // Cập nhật độ dài thanh
        slider.value = t;

        // Chuyển màu từ vàng → xám
        fillImage.color = Color.Lerp(startColor, endColor, t);

        if (t >= 1f && t < 0f)
        {
            isRunning = false;
        }

        UpdateTimeText();
    }

    // Gọi hàm này khi bắt đầu đếm giờ
    public void StartTimer(float time)
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;

        duration = time;
        maxtime = time;
        currentTime = time;
        UpdateTimeText();

        fillImage.color = startColor;

        timer = 0f;
        isRunning = true;
    }

    public void PauseTime()
    {
        isRunning = false;
    }

    public void ContinueTime()
    {
        isRunning = true;
    }

    public void TimeReversal(bool val)
    {
        isReversing = val;
    }

    public void ResetTimer()
    {
        timer = 0f;
        slider.value = 0;
        fillImage.color = startColor;
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
