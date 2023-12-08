using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] List<TileData> tileDatas;
    [SerializeField] int saveLoadLevel;

    LevelData loadLevelData;



    public bool LoadLevel(int level, out Vector2Int startPos) 
    {
        saveLoadLevel = level;
        bool isLoad = LoadLevelData();
        startPos = Vector2Int.zero;
        if (isLoad) 
        {
            startPos = loadLevelData.maps.Find(x => x.eType == Map.EType.Start).coord;
        }
        return isLoad;
    }

    public bool IsContainTile(Map.EType eType, Vector2Int checkCoord) 
    {
        return loadLevelData.maps.Exists(x => x.eType == eType && x.coord == checkCoord);
    }



    #region 타일맵 에디터

    [ContextMenu("SaveLevelData")]
    void SaveLevelData() 
    {
        LevelData levelData = BuildLevelData();
        string jsonData = JsonUtility.ToJson(levelData);
        string path = @$"{Application.dataPath}\Resources\Levels\Level{saveLoadLevel}.json";
        File.WriteAllText(path, jsonData);
        Debug.Log($"Level{saveLoadLevel}.json에 {jsonData.Length}크기만큼 저장됨");
    }

    [ContextMenu("LoadLevelData")]
    bool LoadLevelData() 
    {
        string path = @$"{Application.dataPath}\Resources\Levels\Level{saveLoadLevel}.json";

        if (!File.Exists(path)) 
        {
            Debug.LogError($"{path}에 데이터가 존재하지 않습니다.");
            return false;
        }

        string jsonData = File.ReadAllText(path);
        loadLevelData = JsonUtility.FromJson<LevelData>(jsonData);
        Debug.Log($"Level{saveLoadLevel}.json에서 {jsonData.Length}크기만큼 불러옴");

        ClearView();
        CreateView(loadLevelData);

        return true;
    }

    [ContextMenu("ClearView")]
    void ClearView()
    {
        tilemap.ClearAllTiles();
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }


    T ParseEnum<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }

    LevelData BuildLevelData()
    {
        List<Map> maps = new();

        // 타일맵에서 추가
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            Map map = new Map
            {
                eType = ParseEnum<Map.EType>(tilemap.GetTile(pos).name),
                coord = new Vector2Int(pos.x, pos.y)
            };
            maps.Add(map);
        }

        // 자식 오브젝트에서 추가
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            Map map = new Map
            {
                eType = ParseEnum<Map.EType>(obj.name),
                coord = new Vector2Int(Mathf.RoundToInt(obj.transform.position.x), Mathf.RoundToInt(obj.transform.position.y))
            };
            if (maps.Contains(map)) continue;
            maps.Add(map);
        }

        return new LevelData() { maps = maps };
    }

    void CreateView(LevelData levelData)
    {
        foreach (Map map in levelData.maps)
        {
            TileData tileData = tileDatas.Find(x => x.eType == map.eType);
            Vector3Int coord = new Vector3Int(map.coord.x, map.coord.y, 0);
            if (tileData.isObject)
            {
                GameObject obj = Instantiate(tileData.prefab, coord, Quaternion.identity, transform);
                obj.name = map.eType.ToString();
            }
            else
            {
                tilemap.SetTile(coord, tileData.tileBase);
            }
        }
    }

    #endregion
}
