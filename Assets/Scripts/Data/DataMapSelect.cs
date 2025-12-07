using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataMapSelect : MonoBehaviour
{
    [SerializeField] private int mapID;
    [SerializeField] private Button btnPlay;
    public bool isLock = true;

    [SerializeField] private GameObject lockMap;
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController run;
    [SerializeField] private Image image;
 
    private void Start()
    {
        if(isLock)
        {
            image.color = Color.gray;
        }

        // Dang bi loi !!!
        //else
        //{
        //    UnLock();
        //}

        btnPlay.onClick.AddListener(() =>
        {
            if (isLock) return;
         GameManagerInMap.Instance.StartGame(mapID);
        });
    }

    public void UnLock()
    {
        isLock = false;
        //animator.runtimeAnimatorController = run;
        lockMap.SetActive(false);
        image.color = Color.white;
    }
}
