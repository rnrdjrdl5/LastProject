using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerManaPoint
{
    void SetAwake()
    {
            // UICanvas 받아오기
            UICanvas = GameObject.Find("UICanvas");

            // 패널 받아오기
            MPPanel = UICanvas.transform.Find("MPPanel").gameObject;

            // HP이미지 받아오기
            NowMPImage = MPPanel.transform.Find("NowMpImage").GetComponent<Image>();

            UIManager.GetInstance().mPPanelScript.SetPlayerMana(gameObject);

    }

    void ReTimeMana()
    {
        
        if(isReTimeMana)
        {
            if(NowManaPoint < 100)
            {
                NowManaPoint += Time.deltaTime * ManaRetimePoint;
                if(NowManaPoint >= 100)
                {
                    NowManaPoint = 100;
                    Debug.Log("고정실시");
                }
            }

        }

        Debug.Log("이벤트 전 고유 사용");
        ManaEvent();
    }

    public void CalcManaPoint(float manapoint)
    {
        NowManaPoint -= manapoint;
    }
}
