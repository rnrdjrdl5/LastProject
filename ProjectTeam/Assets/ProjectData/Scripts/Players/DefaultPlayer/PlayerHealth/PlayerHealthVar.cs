using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerHealth
{
    
    /**** private ****/

    private float MaxHealth = 100.0f;   // 최대체력
    private float NowHealth = 100.0f;   // 현재체력

    private PlayerCamera playerCamera;              // 카메라 오브젝트
    private PlayerUI playerUI;                       // UI 오브젝트

    private PhotonManager photonManager;                // 포톤매니저
   
    /**** 접근자 ****/

    public void SetMaxHealth(float MH) { MaxHealth = MH; }
    public void SetNowHealth(float NH) { NowHealth = NH; }

    public float GetMaxHealth() { return MaxHealth; }
    public float GetNowHealth() { return NowHealth; }





}
