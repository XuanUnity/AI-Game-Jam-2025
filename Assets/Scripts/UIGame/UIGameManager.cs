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
    }

    public void SetActiveUIMainGame(bool isActive)
    {
        UIGameMain.SetActive(isActive);
        menuSetting.SetActive(isActive);
    }
}
