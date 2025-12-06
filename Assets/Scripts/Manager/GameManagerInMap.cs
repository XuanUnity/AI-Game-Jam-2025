using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInMap : Singleton<GameManagerInMap>
{
    [SerializeField] private DataMap dataMap;

    [Header("Game Object")]
    [SerializeField] private GameObject uiInGame;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> mapObjects;
    private GameObject currentMap;
    public void StartGame(int mapID)
    {
        UIGameManager.Instance.SetActiveUIMainGame(false);
        MapData selectedMap = dataMap.maps.Find(map => map.mapID == mapID);
        currentMap = GetMapByID(mapID);

        SetActiveMap(currentMap);

        OnActivePlayer(currentMap);
        PlayerController.Instance.InitState(selectedMap);
        uiInGame.SetActive(true);
    }

    public void EndGame()
    {
        uiInGame.SetActive(false);
        player.SetActive(false);
        currentMap.SetActive(false);
        currentMap = null;
        UIGameManager.Instance.SetActiveUIMainGame(true);
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
