using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataMapSelect : MonoBehaviour
{
    [SerializeField] private int mapID;
    [SerializeField] private Button btnPlay;

    private void Start()
    {
        btnPlay.onClick.AddListener(() =>
        {
            GameManagerInMap.Instance.StartGame(mapID);
        });
    }
}
