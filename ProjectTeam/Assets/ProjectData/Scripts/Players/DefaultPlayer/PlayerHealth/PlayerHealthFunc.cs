using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerHealth
{
    public void DestroyPlayer()
    {

        if (photonView.isMine)
        {

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void CallApplyDamage(float _damage)
    {
        gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, _damage);
    }

    public void PlayerDead()
    {
        // 카메라 비활성화
        playerCamera.isPlayerSpawn = false;

        // UI 파괴
        Destroy(playerUI.GetUIObject());

        // 캐릭터 파괴
        Debug.Log("asdfdsaf");
        PhotonNetwork.Destroy(gameObject);

        // Photon매니저에서 승리,패배조건 판단합니다.
        photonManager.GamePlayers.Remove(gameObject);



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
