using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartManager : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnExit;

    [Header("Setting Panel")]
    [SerializeField] private GameObject panelMenuSetting;
    [SerializeField] private RectTransform menuRect;   // RectTransform của MenuSetting
    [SerializeField] private Button btnBack;           // Nút nền mờ full màn hình

    [Header("Sliders")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    private bool isMoving = false;
    private Vector2 hiddenPos = new Vector2(-650f, 0f);
    private Vector2 showPos = new Vector2(0f, 0f);

    private void Start()
    {
        // Vị trí ban đầu: ẩn menu
        menuRect.anchoredPosition = hiddenPos;
        panelMenuSetting.SetActive(false);
        btnBack.interactable = false;

        // ========== BUTTON EVENTS ==========
        btnPlay.onClick.AddListener(() =>
        {
            if (isMoving) return;
            SceneManager.LoadScene("MainScene");
        });

        btnSetting.onClick.AddListener(() =>
        {
            if (isMoving) return;
            OpenMenu();
        });

        btnExit.onClick.AddListener(() =>
        {
            if (isMoving) return;
            Application.Quit();
        });

        btnBack.onClick.AddListener(() =>
        {
            if (isMoving) return;
            CloseMenu();
        });

        // ========== SLIDER EVENTS ==========
        sliderMusic.onValueChanged.AddListener(SetMusicVolume);
        sliderSound.onValueChanged.AddListener(SetSoundVolume);

        // Load volume đã lưu
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sliderSound.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    // ================= MENU ANIMATION =================

    private void OpenMenu()
    {
        isMoving = true;
        panelMenuSetting.SetActive(true);
        btnBack.interactable = false;

        menuRect.anchoredPosition = hiddenPos;

        menuRect.DOAnchorPos(showPos, 1f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                isMoving = false;
                btnBack.interactable = true;
            });
    }

    private void CloseMenu()
    {
        isMoving = true;
        btnBack.interactable = false;

        menuRect.DOAnchorPos(hiddenPos, 1f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                panelMenuSetting.SetActive(false);
                isMoving = false;
            });
    }

    // ================= AUDIO SETTINGS =================

    private void SetMusicVolume(float value)
    {
        // Gắn cho AudioSource nhạc nền
        AudioListener.volume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void SetSoundVolume(float value)
    {
        // Tuỳ bạn xử lý từng AudioSource effect riêng
        PlayerPrefs.SetFloat("SoundVolume", value);
    }
}
