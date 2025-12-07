using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLose : MonoBehaviour
{
    [Header("Pannels Win")]
    [SerializeField] private GameObject pannelWin;
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnRetry;

    [Header("Pannels Lose")]
    [SerializeField] private GameObject pannelLose;
    [SerializeField] private Button btnBackLose;
    [SerializeField] private Button btnRetryLose;

    private void Awake()
    {
        btnBack.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            GameManagerInMap.Instance.EndGame();
            pannelWin.SetActive(false);
        });
        btnBackLose.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            GameManagerInMap.Instance.EndGame();
            pannelLose.SetActive(false);
        });
        btnRetry.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            GameManagerInMap.Instance.RestartGame();
            pannelWin.SetActive(false);
        });
        btnRetryLose.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            GameManagerInMap.Instance.RestartGame();
            pannelLose.SetActive(false);
        });
        btnNext.onClick.AddListener(() =>
        {
            AudioManagerMain.Instance.PlayButtonClick();
            GameManagerInMap.Instance.NextLevelGame();
            pannelWin.SetActive(false);
        });
    }

    public void Win()
    {
        pannelWin.SetActive(true);
    }

    public void Lose()
    {
        pannelLose.SetActive(true);
    }
}
