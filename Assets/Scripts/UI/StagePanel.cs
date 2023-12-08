using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
	public delegate void ClickStageBtnEvent(StageData stageData);
	public event ClickStageBtnEvent clickStageBtn;


	[SerializeField] GameObject parent;
	[SerializeField] Transform stageBtnParent;
	[SerializeField] GameObject stageBtnPrefab;

	List<StageBtn> stageBtns;



	public void Show() 
	{
		parent.SetActive(true);
		RenewStageBtns();

	}

	public void Hide() 
	{
		parent.SetActive(false);
	}

	public void InitStageBtns(List<StageData> stageDatas) 
	{
		stageBtns = new();
		foreach (StageData stageData in stageDatas)
		{
			StageBtn stageBtn = Instantiate(stageBtnPrefab, stageBtnParent).GetComponent<StageBtn>();
			stageBtn.Init(stageData);
			stageBtn.GetComponent<Button>().onClick.AddListener(() => ClickStageBtn(stageData));
			stageBtns.Add(stageBtn);
		}
	}


	void RenewStageBtns() 
	{
		foreach (StageBtn stageBtn in stageBtns)
		{
			stageBtn.Renew();
		}
	}

	void ClickStageBtn(StageData stageData) 
	{
		if (stageData.isLock) return;
		clickStageBtn?.Invoke(stageData);
	}
}
