using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private GameObject step1;
    [SerializeField] private GameObject step2;
    [SerializeField] private GameObject step3;
    [SerializeField] private GameObject step4;
    [SerializeField] private GameObject step5;
    [SerializeField] private GameObject step6;

    [SerializeField] private Button btnSkip;

    public bool isTutorial;

    private void Start()
    {
        btnSkip.onClick.AddListener(() =>
        {
            isTutorial = false;
            step1.SetActive(false);
            step2.SetActive(false);
            step3.SetActive(false);
            step4.SetActive(false);
            step5.SetActive(false);
            step6.SetActive(false);
            GameManagerInMap.Instance.StartMap1();
        });


        isTutorial = PlayerPrefs.GetInt("FirstTime", 1) == 1;
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);
        step5.SetActive(false);
        step6.SetActive(false);

        if (isTutorial)
        {
            step1.SetActive(true);
            PlayerPrefs.SetInt("FirstTime", 0);
        }
        else
        {
            step1.SetActive(false);
        }
    }
    public int indexStep = 1;
    private void Update()
    {
        if (!isTutorial) return;

        switch(indexStep)
        {
            case 2:
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    step2.SetActive(false);
                    GameManagerInMap.Instance.ContinueGame();
                    indexStep = 3;
                }
                break;
            case 3:
                if (PlayerController.Instance.IsInLight)
                {
                    step3.SetActive(true);
                    GameManagerInMap.Instance.PauseGame();
                    indexStep = 4;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                { 
                    GameManagerInMap.Instance.ContinueGame();
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        step3.SetActive(false);
                        step4.SetActive(true);
                        GameManagerInMap.Instance.PauseGame();
                        indexStep = 5;
                    });
                    
                }
                break;
            case 5:
                if (Input.GetKey(KeyCode.F))
                {
                    step4.SetActive(false);
                    GameManagerInMap.Instance.ContinueGame();

                    DOVirtual.DelayedCall(1f, () =>
                    {
                        step5.SetActive(true);
                        GameManagerInMap.Instance.PauseGame();
                        indexStep = 6;
                    });
                }
                break;
            case 6:
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    DOVirtual.DelayedCall(0.6f, () =>
                    {
                        GameManagerInMap.Instance.ContinueGame();
                        step5.SetActive(false);
                        GameManagerInMap.Instance.PauseGame();
                        step6.SetActive(true);
                        indexStep = 7;
                    });
                }
                break;
            case 7:
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                { 
                    DOVirtual.DelayedCall(0.6f, () =>
                    {
                        GameManagerInMap.Instance.ContinueGame();
                        step6.SetActive(false);
                    });
                    
                }
                break;
        }
    }

    public void OnTutorial()
    {
        isTutorial = true;
        indexStep = 2;
        step1.SetActive(false);
        step2.SetActive (true);
        GameManagerInMap.Instance.PauseGame();
    }
}
