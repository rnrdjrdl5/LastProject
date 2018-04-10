﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerHealth
{
    

    private void SetAwake()
    {
        if (photonView.isMine)
        {

            // UICanvas 받아오기
            UICanvas = GameObject.Find("UICanvas");

            // 패널 받아오기
            HPPanel = UICanvas.transform.Find("HPPanel").gameObject;

            // HP이미지 받아오기
            NowHPImage = HPPanel.transform.Find("NowHpImage").GetComponent<Image>();
            Debug.Log(NowHPImage);

            // 카메라 설정
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

            // 이펙트 매니저 설정
            playerEffectManager = GetComponent<PlayerEffectManager>();

            isHiting = false;
            NowHiting = 0.0f;
        }
    }


    public void CallApplyDamage(float _damage ,PlayerEffectManager.EnumPlayerEffect EPE)
    {
        photonView.RPC("ApplyDamage", PhotonTargets.All, _damage , (int)EPE);
    }

    public void PlayerDead()
    {
        
        Debug.Log("플레이어사망");
        // 카메라 비활성화
        playerCamera.isPlayerSpawn = false;

        // 플레이어 타입 죽은 상태로 전환
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { {"PlayerType","Dead"} });

        // 캐릭터 파괴
        PhotonNetwork.Destroy(gameObject);

    }



    /**** RPC ****/

    [PunRPC]
    private void ApplyDamage(float _damage , PlayerEffectManager.EnumPlayerEffect EPE)
    {

        // 본인 인 경우에만
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            // 데미지 입음
            NowHealth -= _damage;

            Debug.Log(NowHealth);
            // 체력 0이하면 죽음 처리
            if (NowHealth <= 0) {
                NowHPImage.fillAmount = NowHealth / MaxHealth;
                PlayerDead();
                    }
        }

        // 그 외에도 이펙트는 준다.
        for(int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material.color = Color.red;
        }

        isHiting = true;
        NowHiting = 0.0f;

        // 이펙트 사용

        

    }






}
