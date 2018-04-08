using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerHealth : Photon.PunBehaviour, IPunObservable
{

    private void Awake()
    {
        SetAwake();

    }

    public void Update()
    {
        if (photonView.isMine)
        {
            NowHPImage.fillAmount = NowHealth / MaxHealth;
        }
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
