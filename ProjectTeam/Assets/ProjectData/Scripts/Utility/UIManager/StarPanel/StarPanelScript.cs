using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanelScript{

    public int MaxStar = 5;

    private int OneStarCondition = 16;
    private int TwoStarCondition = 32;
    private int ThreeStarCondition = 48;
    private int ForeStarCondition = 64;
    private int FiveStarCondition = 80;




    public GameObject StarPanel;
    private void InitStarPanel() { StarPanel = UIManager.GetInstance().UICanvas.transform.Find("StarPanel").gameObject; }

    public GameObject[] StarImage;
    private void InitStarImage()
    {
        StarImage = new GameObject[MaxStar];
        for (int i = 0; i < MaxStar; i++)
        {

            StarImage[i] = StarPanel.transform.Find("StarImage" + (i + 1).ToString()).gameObject;
        }
    }

    // 특정 번호의 별을 설정
    public void SetStarNumber(bool isActive, int type)
    {
        StarImage[type - 1].SetActive(isActive);
    }

    // 모든 별들에게 공통적으로 실시
    public void SetStar(bool isActive)
    {
        for (int i = 0; i < StarImage.Length; i++)
        {
            StarImage[i].SetActive(isActive);
        }

        StarPanel.SetActive(isActive);
    }

    // 패널 설정
    public void SetStarPanel(bool isActive)
    {
        StarPanel.SetActive(isActive);
    }


    public void InitData()
    {
        InitStarPanel();
        InitStarImage();

        UIManager.GetInstance().UpdateEvent += CheckRestUI;
    }


    public void CheckRestUI()
    {

        float ObjectPersent = ((float)ObjectManager.GetInstance().InterObj.Count 
            / (float)ObjectManager.GetInstance().MaxInterObj) * 100;



        if (ObjectPersent >= FiveStarCondition)
        {
            ObjectPersent = 5;
        }
        else if (ObjectPersent >= ForeStarCondition && ObjectPersent < FiveStarCondition)
        {
            ObjectPersent = 4;
        }
        else if (ObjectPersent >= ThreeStarCondition && ObjectPersent < ForeStarCondition)
        {
            ObjectPersent = 3;
        }
        else if (ObjectPersent >= TwoStarCondition && ObjectPersent < ThreeStarCondition)
        {
            ObjectPersent = 2;
        }
        else if (ObjectPersent >= OneStarCondition && ObjectPersent < TwoStarCondition)
        {
            ObjectPersent = 1;
        }
        else
        {
            ObjectPersent = 0;
        }

        for (int i = 0; i < StarImage.Length; i++)
        {
            if (i < ObjectPersent)
                StarImage[i].SetActive(true);

            else
                StarImage[i].SetActive(false);
        }
    }





}
