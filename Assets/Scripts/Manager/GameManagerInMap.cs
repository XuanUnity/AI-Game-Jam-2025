using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInMap : Singleton<GameManagerInMap>
{
    [SerializeField] private DataMap dataMap;
    [SerializeField] private LightController lightController;
    [SerializeField] private UIWinLose uiWinLose;

    [Header("Game Object")]
    [SerializeField] private GameObject uiInGame;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> mapObjects;
    [SerializeField] private List<DataMapSelect> dataMapSelect;
    private GameObject currentMap;
    private int currentMapID;
    private int mapUnLock;

    private void Start()
    {
        this.gameObject.SetActive(true);
        mapUnLock = PlayerPrefs.GetInt("MapUnLock", 0);
        SetMapUnLock();
    }
    public void SetMapUnLock()
    {
        for (int i = 0; i <= mapUnLock; i++)
        {
            if (dataMapSelect[i].isLock)
                dataMapSelect[i].UnLock();
        }
    }

    public void InitLight(LightController controller)
    {
        lightController = controller;
    }

    public void StartGame(int mapID)
    {
        currentMapID = mapID;
        UIGameManager.Instance.SetActiveUIMainGame(false);
        MapData selectedMap = dataMap.maps.Find(map => map.mapID == mapID);
        currentMap = GetMapByID(mapID);

        SetActiveMap(currentMap);

        OnActivePlayer(currentMap);
        PlayerController.Instance.InitState(selectedMap);
        PlayerController.Instance.SetLight(currentMap);
        uiInGame.SetActive(true);
        lightController.StartLight(selectedMap.timeLimit);

        if(mapID == 0 && TutorialManager.Instance.isTutorial)
        {
            GameManagerInMap.Instance.PauseGame();
            TutorialManager.Instance.indexStep = 1;
            TutorialManager.Instance.OnTutorial();
        }
    }

    public void RestartGame()
    {
        if(currentMap == null)
        {
            Debug.Log("Current map is null, cannot restart game.");
        }
        StartGame(currentMapID);
    }

    public void NextLevelGame() // level tiếp theo
    {
        currentMap.SetActive(false);
        if (currentMapID < mapObjects.Count)
        {
            currentMapID++;
            mapUnLock = Mathf.Max(mapUnLock, currentMapID);
            PlayerPrefs.SetInt("MapUnLock", mapUnLock);
            dataMapSelect[currentMapID].UnLock();
        }


        StartGame(currentMapID);
    }

    public void PauseGame()
    {
        PlayerController.Instance.SetPause(true);
        lightController.SetActionLight(true);
    }

    public void PauseLight()
    {
        lightController.SetActionLight(true);
    }

    public void ContinueGame()
    {
        PlayerController.Instance.SetPause(false);
        lightController.SetActionLight(false);
    }

    public void EndGame()
    {
        uiInGame.SetActive(false);
        player.SetActive(false);
        currentMap.SetActive(false);
        currentMap = null;
        UIGameManager.Instance.SetActiveUIMainGame(true);
    }

    public void WinGame()
    {
        uiWinLose.Win();
        PauseGame();
    }

    public void LoseGame()
    {
        uiWinLose.Lose();
        PauseGame();
    }
    public GameObject GetMapByID(int idmap)
    {
        return mapObjects[idmap];
    }

    public void SetActiveMap(GameObject map)
    {
        map.SetActive(true);
    }

    public void OnActivePlayer(GameObject map)
    {
        Transform posStart = map.transform.Find("posStart");
        player.transform.position = posStart.position;
        player.SetActive(true);
    }
}
