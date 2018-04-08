using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public partial class PlayerHealth
{
    /**** public ****/

    public GameObject HPPanel;              // HP 패널
    


    /**** private ****/

    private PlayerCamera playerCamera;              // 카메라 오브젝트

    private GameObject UICanvas;            // UI 캔버스
    private GameObject HPObject;            // HP 오브젝트
    private Image NowHPImage;               // HP 이미지


    private float MaxHealth = 100.0f;   // 최대체력
    private float NowHealth = 100.0f;   // 현재체력


    /**** 접근자 ****/

    public void SetMaxHealth(float MH) { MaxHealth = MH; }
    public void SetNowHealth(float NH) { NowHealth = NH; }

    public float GetMaxHealth() { return MaxHealth; }
    public float GetNowHealth() { return NowHealth; }





}
