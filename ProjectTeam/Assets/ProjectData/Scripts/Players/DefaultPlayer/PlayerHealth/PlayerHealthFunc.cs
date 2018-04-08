using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerHealth
{
    

    private void SetAwake()
    {
        if (photonView.isMine)
        {
            // 플레이어 카메라 초기화
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

            // UICanvas 받아오기
            UICanvas = GameObject.Find("UICanvas");

            // HP패널 생성
            HPObject = Instantiate(HPPanel);
            HPObject.transform.SetParent(UICanvas.transform);

            // 크기 위치 설정
            Vector3 v3 = new Vector3
            {
                x = Screen.width / 2,
                y = Screen.height / 2,
                z = 0.0f
            };

            HPObject.transform.localScale = Vector3.one;
            HPObject.transform.position = v3;



            // 현재 HP 이미지 받아오기
            NowHPImage = HPObject.transform.Find("NowHpImage").GetComponent<Image>();
        }
    }


    public void CallApplyDamage(float _damage)
    {
        gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, _damage);
    }

    public void PlayerDead()
    {
        
        Debug.Log("플레이어사망");
        // 카메라 비활성화
        playerCamera.isPlayerSpawn = false;

        // 플레이어 타입 죽은 상태로 전환
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { {"PlayerType","Dead"} });

        // 모든 플레이어 UI 파괴
        DestroyUI();

        // 캐릭터 파괴
        PhotonNetwork.Destroy(gameObject);

    }

    public void DestroyUI()
    { 
        Destroy(HPObject);
        Destroy(gameObject.GetComponent<PlayerManaPoint>().GetMPObject());
    }


    /**** RPC ****/

    [PunRPC]
    private void ApplyDamage(float _damage)
    {

        // 본인 인 경우에만
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            // 데미지 입음
            NowHealth -= _damage;

            // 체력 0이하면 죽음 처리
            if (NowHealth <= 0) PlayerDead();
        }
    }

    


    
}
