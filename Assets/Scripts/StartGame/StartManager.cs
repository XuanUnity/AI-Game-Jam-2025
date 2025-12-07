using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartManager : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnNewGame;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnExit;

    [Header("Setting Panel")]
    [SerializeField] private GameObject panelMenuSetting;
    [SerializeField] private Button btnBackNewGame;
    [SerializeField] private Button btnConfirmNewGame;
    [SerializeField] private GameObject panelConfirmNewGame;
    [SerializeField] private RectTransform menuRect;   // RectTransform của MenuSetting
    [SerializeField] private RectTransform menuNewGameRect;   // RectTransform của MenuNewGame
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
        panelConfirmNewGame.SetActive(false);
        btnBack.interactable = false;
        btnBackNewGame.interactable = false;
        btnConfirmNewGame.interactable = false;

        if(PlayerPrefs.GetInt("FirstTimePlayed", 0) == 0)
        {
            btnPlay.interactable = false;
        }
        else
        {
            btnPlay.interactable = true;
        }

        // ========== BUTTON EVENTS ==========
        btnPlay.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            videoManager.PlayContinue();
        });
        btnConfirmNewGame.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            videoManager.PlayNewGame();
        });

        btnSetting.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            OpenMenu();
        });

        btnNewGame.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();

            if (PlayerPrefs.GetInt("FirstTimePlayed", 0) == 0)
            {
                if (isMoving) return;
                videoManager.PlayNewGame();
            }
            else
            {
                if (isMoving) return;
                OpenMenuNewGame();
            }
        });
        btnNewGame.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            panelConfirmNewGame.SetActive(true);
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

        btnBackNewGame.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            if (isMoving) return;
            CloseMenuNewGame();
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

    private void OpenMenuNewGame()
    {
        isMoving = true;
        panelConfirmNewGame.SetActive(true);
        btnBackNewGame.interactable = false;

        menuNewGameRect.anchoredPosition = hiddenPos;

        menuNewGameRect.DOAnchorPos(showPos, 1f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                isMoving = false;
                btnBackNewGame.interactable = true;
                btnConfirmNewGame.interactable = true;
            });
    }

    private void CloseMenuNewGame()
    {
        isMoving = true;
        btnBackNewGame.interactable = false;
        btnConfirmNewGame.interactable = false;

        menuNewGameRect.DOAnchorPos(hiddenPos, 1f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                panelConfirmNewGame.SetActive(false);
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
