using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataMapSelect : MonoBehaviour
{
    [SerializeField] private int mapID;
    [SerializeField] private Button btnPlay;
    public bool isLock = true;

    [SerializeField] private GameObject look;
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController run;
    [SerializeField] private Image image;
 
    private void Start()
    {
        if(isLock)
        {
            image.color = Color.gray;
        }
        btnPlay.onClick.AddListener(() =>
        {
            if (isLock) return;

            if(mapID == 0)
            {
                TutorialManager.Instance.isTutorial = true;
            }
            GameManagerInMap.Instance.StartGame(mapID);
        });
    }

    public void UnLock()
    {
        isLock = false;
        //animator.runtimeAnimatorController = run;
        look.SetActive(false);
        image.color = Color.white;
    }
}
