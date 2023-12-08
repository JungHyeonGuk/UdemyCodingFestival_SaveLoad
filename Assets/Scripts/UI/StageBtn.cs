using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageBtn : MonoBehaviour
{
    [SerializeField] Text stageText;
    [SerializeField] Image btn;
    [SerializeField] Color[] colors;
    [SerializeField] Image[] starIcons;

	StageData stageData;


	public void Init(StageData stageData)
	{
		this.stageData = stageData;
		stageText.text = stageData.level.ToString();
		btn.color = colors[stageData.isLock ? 2 : 0];

		for (int i = 0; i < starIcons.Length; i++)
		{
			bool isActiveStar = i < stageData.star;
			starIcons[i].gameObject.SetActive(!stageData.isLock);
			starIcons[i].color = colors[isActiveStar ? 0 : 1];
		}
	}

	public void Renew() 
	{
		Init(stageData);
	}
}
