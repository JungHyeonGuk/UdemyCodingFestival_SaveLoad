using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] SaveManager saveManager;
    [SerializeField] TileManager tileManager;
    [SerializeField] CamManager camManager;
    [SerializeField] Player player;
    [SerializeField] StagePanel stagePanel;
    [SerializeField] WinPanel winPanel;
    [SerializeField] LosePanel losePanel;

    [SerializeField] bool isGamePlay;
    [SerializeField] int gainStar;
    [SerializeField] int gainCoin;
    [SerializeField] StageData curStageData;



    public void ShowPanel(string panelName)
    {
        if (panelName == "StagePanel") stagePanel.Show();
        else stagePanel.Hide();

        if (panelName == "WinPanel") winPanel.Show();
        else winPanel.Hide();

        if (panelName == "LosePanel") losePanel.Show();
        else losePanel.Hide();
    }




    Vector3 GetMoveTarget(Vector2Int curPos, Vector2Int dir) 
	{
        Vector2Int target = curPos;
        for (int i = 0; i < 1000; i++)
        {
            Vector2Int checkPos = curPos + dir * i;
            if (tileManager.IsContainTile(Map.EType.Wall, checkPos)) break;
            if (tileManager.IsContainTile(Map.EType.End, checkPos)) 
            {
                target = checkPos;
                break;
            }
            target = checkPos;
        }
        return new Vector3(target.x, target.y, 0);
    }

    void InputMove(Vector2Int dir) 
    {
        if (player.IsMove()) return;

        Vector3 targetPos = GetMoveTarget(player.GetPos(), dir);
        player.MoveToTarget(targetPos);
        player.RotateByDir(dir);
    }

    void WinStage() 
    {
        isGamePlay = false;
        curStageData.star = gainStar;
        saveManager.Coin += gainCoin;

        StageData nextStageData = saveManager.StageDatas.Find(x => x.level == curStageData.level + 1);
        if (nextStageData != null) nextStageData.isLock = false;

        winPanel.Init(curStageData, gainStar, gainCoin);
        ShowPanel("WinPanel");
        saveManager.SaveData();
    }

    void LoseStage() 
    {
        isGamePlay = false;
        losePanel.Init(curStageData);
        ShowPanel("LosePanel");
    }




    void PlayerOnTriggerEnter(GameObject other)
    {
        if (other.name == Map.EType.Coin.ToString())
        {
            ++gainCoin;
            Destroy(other);
        }
        else if (other.name == Map.EType.Star.ToString())
        {
            ++gainStar;
            Destroy(other);
        }
        else if (other.name == Map.EType.End.ToString()) 
        {
            WinStage();
        }
        else if (other.name.Contains("Thorn"))
        {
            LoseStage();
        }
    }

    void SaveManagerOnLoad()
    {
        stagePanel.InitStageBtns(saveManager.StageDatas);
        ShowPanel("StagePanel");
    }

    void StagePanelOnClickStageBtn(StageData stageData)
    {
        // 스테이지 클릭하여 게임 시작
        ShowPanel("None");

        isGamePlay = true;
        gainCoin = 0;
        gainStar = 0;
        curStageData = stageData;

        if (tileManager.LoadLevel(stageData.level, out Vector2Int startPos))
		{
			player.SetPos(startPos);
			player.RotateByDir(new Vector2Int(0, -1));
            camManager.MoveToTargetInstant(player.transform.position);
        }
	}


    void Awake()
	{
        player.triggerEnter += PlayerOnTriggerEnter;
		saveManager.load += SaveManagerOnLoad;
		stagePanel.clickStageBtn += StagePanelOnClickStageBtn;
    }


	void Update()
	{
        if (!isGamePlay) return;

        // 상하좌우 키 입력
        if (Input.GetKeyDown("left")) InputMove(new Vector2Int(-1, 0));
        else if (Input.GetKeyDown("right")) InputMove(new Vector2Int(1, 0));
        else if (Input.GetKeyDown("down")) InputMove(new Vector2Int(0, -1));
        else if (Input.GetKeyDown("up")) InputMove(new Vector2Int(0, 1));
    }

	void LateUpdate()
	{
        if (!isGamePlay) return;

        camManager.MoveToTarget(player.transform.position);
    }

	void OnDestroy()
	{
        player.triggerEnter -= PlayerOnTriggerEnter;
        saveManager.load -= SaveManagerOnLoad;
        stagePanel.clickStageBtn -= StagePanelOnClickStageBtn;
    }
}
