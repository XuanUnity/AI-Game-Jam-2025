using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataMap", menuName = "Data/DataMap")]
public class DataMap : ScriptableObject
{
    public List<MapData> maps;
}

[System.Serializable]
public class MapData
{
    public string mapName;
    public int mapID;
    public float timeLimit;
    public float healthPlayer;
    public float enrgyPlayer;
}
