using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameManager : MonoBehaviour
{
    [Header("Button Settings In Game")]
    [SerializeField] private Button btnSetting;

    [Header("Settings Menu")]
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnRetry;
    [SerializeField] private Button btnHome;

    [Header("Settings Music")]
    [SerializeField] private Button btnBackSun;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderEffect;

    [Header("Pannels")]
    [SerializeField] private GameObject pannelMenuSetting;
    [SerializeField] private GameObject pannelMenuMusic;

    private void Awake()
    {
        //Nhan setting -> bat pannel setting
        btnSetting.onClick.AddListener(() => { 
            GameManagerInMap.Instance.PauseGame();
            pannelMenuSetting.SetActive(true);
        });

        //Trong menu setting
        btnBack.onClick.AddListener(() => {
            GameManagerInMap.Instance.ContinueGame();
            pannelMenuSetting.SetActive(false);
        });
        btnSettings.onClick.AddListener(() => {
            pannelMenuMusic.SetActive(true);
            pannelMenuSetting.SetActive(false);
        });
        btnRetry.onClick.AddListener(() => {
            //reset map
            GameManagerInMap.Instance.RestartGame();
            pannelMenuSetting.SetActive(false);
        });
        btnHome.onClick.AddListener(() => {
            //ve home
            GameManagerInMap.Instance.EndGame();
            pannelMenuSetting.SetActive(false);
        });

        //Trong menu setting music
        btnBackSun.onClick.AddListener(() => {
            pannelMenuMusic.SetActive(false);
            pannelMenuSetting.SetActive(true);
        });

    }
}
