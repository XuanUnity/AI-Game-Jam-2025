using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameManager : Singleton<UIGameManager>
{
    [SerializeField] private Button btnBack;
    [SerializeField] private GameObject UIGameMain;

    private void Start()
    {
        btnBack.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("StartScene");
        });
    }

    public void SetActiveUIMainGame(bool isActive)
    {
        UIGameMain.SetActive(isActive);
    }
}
