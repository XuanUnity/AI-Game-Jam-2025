using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameManager : Singleton<UIGameManager>
{
    [SerializeField] private Button btnBack;
    [SerializeField] private GameObject UIGameMain;
    [SerializeField] private Button Settings;
    [SerializeField] private GameObject panelSetting;

    [SerializeField] private Button btnBackMenu;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    [SerializeField] private GameObject menuSetting;

    private void Start()
    {
        btnBack.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            SceneManager.LoadScene("StartScene");
        });
        Settings.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            panelSetting.SetActive(true);
        });
        btnBackMenu.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            panelSetting.SetActive(false);
        });

        // ========== SLIDER EVENTS ==========
        sliderMusic.onValueChanged.AddListener(SetMusicVolume);
        sliderSound.onValueChanged.AddListener(SetSoundVolume);

        // Load volume đã lưu
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sliderSound.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    public void SetActiveUIMainGame(bool isActive)
    {
        UIGameMain.SetActive(isActive);
        menuSetting.SetActive(isActive);
    }

    private void SetMusicVolume(float value)
    {
        AudioManagerMain.Instance.SetBGMVolume(value);
    }

    private void SetSoundVolume(float value)
    {
        AudioManagerMain.Instance.SetSFXVolume(value);
    }

}
