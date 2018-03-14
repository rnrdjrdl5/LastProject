using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerHealth
{
    public void DestroyPlayer()
    {
        if (photonView.isMine)
        { PhotonNetwork.Destroy(gameObject); }
    }

    void SendHealthData(PhotonStream stream)
    {
        if (stream.isWriting)
        {
            stream.SendNext(NowHealth);
        }
    }

    void RecvHealthData(PhotonStream stream)
    {
        if (!stream.isWriting)
        {
            RecvHealth = (float)stream.ReceiveNext();
        }
    }

    public void CallApplyDamage(float _damage)
    {
        gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, _damage);
    }

    public void SyncHealth()
    {
        if (!gameObject.GetComponent<PhotonView>().isMine)
        {
            NowHealth = RecvHealth;
        }
    }

    /*private void OnDeadZone(Collider other)
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (other.tag == "DeadZone")
            {
                PlayerDead();
                Debug.Log("플레이어 데드존 입장");


            }
        }
    }*/

    public void PlayerDead()
    {

        if (gameObject.tag == "Teams")
        {
            PhotonManager pm = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
            pm.CallSetTeamsCount(-1);
        }
        else if (gameObject.tag == "Boss")
        {
            PhotonManager pm = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
            pm.CallSetBossCount(-1);
        }

        GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().isPlayerSpawn = false;

        PhotonNetwork.Destroy(gameObject);

        Destroy(gameObject.GetComponent<PlayerUI>().GetUIObject());



    }

    // 밑의 두 함수는 임시용 함수. 수정해야함.

    void isDead()
    {
        if(gameObject.GetComponent<PhotonView>().isMine)
        {
            if (NowHealth <= 0)
                PlayerDead();
        }
    }



    



    




    /// <summary>
    /// 이 아래부터는 RPC입니다.
    /// </summary>
    /// <param name="_damage"></param>
    [PunRPC]
    private void ApplyDamage(float _damage)
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            NowHealth -= _damage;
            Debug.Log("데미지입음 , 데미지 : " + _damage);
            Debug.Log(" 남은 체력 : " + NowHealth);
        }
    }

    
}
