using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct Map
{
    public enum EType
    {
        None,
        /// <summary> 벽 </summary>
        Wall,
        /// <summary> 시작 </summary>
        Start,
        /// <summary> 도착 </summary>
        End,
        /// <summary> 동전 </summary>
        Coin,
        /// <summary> 별 </summary>
        Star,
        /// <summary> 가시 </summary>
        ThornUp,
        ThornDown,
        ThornRight,
        ThornLeft,
    }

    public EType eType;
    public Vector2Int coord;
}

[Serializable]
public struct LevelData
{
    public List<Map> maps;
}

[Serializable]
public class TileData
{
    public Map.EType eType;
    public bool isObject;
    public TileBase tileBase;
    public GameObject prefab;
}

[Serializable]
public class StageData 
{
    public int level;
    public int star;
    public bool isLock;
}

[Serializable]
public class SaveModel 
{
    public int coin;
    public List<StageData> stageDatas;
}
