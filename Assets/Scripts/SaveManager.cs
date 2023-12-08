using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public delegate void LoadEvent();
    public event LoadEvent load;

    [SerializeField] SaveModel saveModel;

    public int Coin 
    {
        get => saveModel.coin;
        set => saveModel.coin = value;
    }
    public List<StageData> StageDatas 
    {
        get => saveModel.stageDatas;
        set => saveModel.stageDatas = value;
    }


    string SavePath
    {
        get => @$"{Application.persistentDataPath}\SaveData.json";
    }




    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(saveModel);
        File.WriteAllText(SavePath, jsonData);
        Debug.Log($"{jsonData}∏∏≈≠ ¿˙¿Âµ ");
    }


    void LoadData()
    {
        string jsonData = File.ReadAllText(SavePath);
        saveModel = JsonUtility.FromJson<SaveModel>(jsonData);

        Debug.Log($"∑ŒµÂµ  coin: {Coin}, stageDatas:{StageDatas.Count}");
    }

    void VeryFirstLoadData()
    {
        Coin = 0;
        StageDatas = new();

        for (int i = 1; i < 100000; i++)
        {
            string levelPath = @$"{Application.dataPath}\Resources\Levels\Level{i}.json";
            if (!File.Exists(levelPath)) break;
            StageData stageData = new StageData() { level = i, star = 0, isLock = i > 1 };
            StageDatas.Add(stageData);
        }

        Debug.Log($"√÷√  ∑ŒµÂµ  coin: {Coin}, stageDatas:{StageDatas.Count}");
    }


    void Start()
    {
        if (File.Exists(SavePath)) LoadData();
        else VeryFirstLoadData();

        load?.Invoke();
    }


    [ContextMenu("ClearPersistFile")]
    void ClearPersistFile() 
    {
        File.Delete(SavePath);
    }
}
