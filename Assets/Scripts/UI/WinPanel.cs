using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Text stageText;
    [SerializeField] Text coinText;
    [SerializeField] Image[] starIcons;
    [SerializeField] Color[] colors;



    public void Init(StageData stageData, int gainStar, int gainCoin) 
    {
        stageText.text = $"스테이지 {stageData.level}";
		for (int i = 0; i < starIcons.Length; i++)
		{
            bool isActiveStar = i < gainStar;
            starIcons[i].color = colors[isActiveStar ? 0 : 1];
        }
        coinText.text = $"얻은코인 {gainCoin}";
    }


    public void Show() 
    {
        parent.SetActive(true);
    }

    public void Hide()
    {
        parent.SetActive(false);
    }
}
