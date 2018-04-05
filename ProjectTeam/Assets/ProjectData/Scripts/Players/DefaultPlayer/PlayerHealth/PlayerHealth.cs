using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerHealth : Photon.PunBehaviour, IPunObservable
{

    private void Awake()
    {
        // 플레이어 카메라 초기화
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();

        // 플레이어 UI 초기화
        playerUI = gameObject.GetComponent<PlayerUI>();

        // 포톤 매니저 받아옴.

        GameObject go = GameObject.Find("PhotonNetwork");

        if(go != null) 
            photonManager = go.GetComponent<PhotonManager>();


    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        
        if (stream.isWriting)
        {
            stream.SendNext(NowHealth);
        }

        else
            NowHealth = (float)stream.ReceiveNext();

    }



    


}
