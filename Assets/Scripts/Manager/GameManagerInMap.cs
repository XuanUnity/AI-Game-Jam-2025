using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInMap : MonoBehaviour
{
    [SerializeField] private DataMap dataMap;
    public void StartGame(int mapID)
    {
        MapData selectedMap = dataMap.maps.Find(map => map.mapID == mapID);
        PlayerController.Instance.InitState(selectedMap);
    }
}
