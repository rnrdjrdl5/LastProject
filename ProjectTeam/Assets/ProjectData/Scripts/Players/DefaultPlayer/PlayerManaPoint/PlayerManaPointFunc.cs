using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerManaPoint
{
    void SetAwake()
    {
        if (gameObject.GetPhotonView().isMine)
        {

            // UICanvas 받아오기
            UICanvas = GameObject.Find("UICanvas");

            // MP패널 생성
            MPObject = Instantiate(MPPanel);
            MPObject.transform.SetParent(UICanvas.transform);

            // 크기 위치 설정
            Vector3 v3 = new Vector3
            {
                x = Screen.width / 2,
                y = Screen.height / 2,
                z = 0.0f
            };

            MPObject.transform.localScale = Vector3.one;
            MPObject.transform.position = v3;



            // 현재 MP 이미지 받아오기
            NowMPImage = MPObject.transform.Find("NowMpImage").GetComponent<Image>();
        }
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
    }

    public void CalcManaPoint(float manapoint)
    {
        NowManaPoint -= manapoint;
    }
}
