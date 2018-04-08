using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerManaPoint
{
    public GameObject MPPanel;              // HP 패널

    private PlayerCamera playerCamera;              // 카메라 오브젝트

    private GameObject UICanvas;            // UI 캔버스
    private GameObject MPObject;            // MP 오브젝트
    private Image NowMPImage;               // MP 이미지

    public GameObject GetMPObject() { return MPObject; }


    private float MaxManaPoint = 100.0f;

    public void SetMaxManaPoint(float MP) { MaxManaPoint = MP; }
    public float GetMaxManaPoint() { return MaxManaPoint; }

    private float NowManaPoint = 100.0f;

    public void SetNowManaPoint(float MP) { NowManaPoint = MP; }
    public float GetNowManaPoint() { return NowManaPoint; }

    private bool isReTimeMana = true;
    
    public void SetisReTimeMana(bool s) { isReTimeMana = s; }
    public bool GetisReTimeMana() { return isReTimeMana; }

    public void DecreaseMana(float MP)
    {
        NowManaPoint -= MP;
        if (NowManaPoint < 0)
            NowManaPoint = 0;
    }


    [Header(" - 마나 리젠속도")]
    [Tooltip(" - 마나 리젠속도입니다. ")]
    public float ManaRetimePoint = 5.0f;




}
