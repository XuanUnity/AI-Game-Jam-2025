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

    [SerializeField] private VideoManager videoManager;

    private void Start()
    {
        // Vị trí ban đầu: ẩn menu
        menuRect.anchoredPosition = hiddenPos;
        panelMenuSetting.SetActive(false);
        btnBack.interactable = false;

        // ========== BUTTON EVENTS ==========
        btnPlay.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            videoManager.OnPlayClicked();
        });

        btnSetting.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            OpenMenu();
        });

        btnExit.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            Application.Quit();
        });

        btnBack.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            CloseMenu();
        });

        // ========== SLIDER EVENTS ==========
        sliderMusic.onValueChanged.AddListener(SetMusicVolume);
        sliderSound.onValueChanged.AddListener(SetSoundVolume);

        // Load volume đã lưu
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sliderSound.value = PlayerPrefs.GetFloat("SoundVolume", 1f);

        //Start BGM Source
        AudioStartManager.Instance.PlayBGM(true);
    }

    // ================= MENU ANIMATION =================

    private void OpenMenu()
    {
        isMoving = true;
        panelMenuSetting.SetActive(true);
        btnBack.interactable = false;

        menuRect.anchoredPosition = hiddenPos;

        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sliderSound.value = PlayerPrefs.GetFloat("SoundVolume", 1f);

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
        AudioStartManager.Instance.SetBGMVolume(value);
    }

    private void SetSoundVolume(float value)
    {
        AudioStartManager.Instance.SetSFXVolume(value);
    }
}
