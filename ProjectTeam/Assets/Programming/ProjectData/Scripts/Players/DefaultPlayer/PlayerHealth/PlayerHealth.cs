using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerHealth : Photon.PunBehaviour, IPunObservable
{
    
    private void Awake()
    {
        if(gameObject.GetPhotonView().isMine)
            SetAwake();

    }

    public void Update()
    {
        if (isHiting)
        {
            if (NowHiting + Time.deltaTime >= MaxHiting)
            {
                isHiting = false;
                NowHiting = 0.0f;

                for (int i = 0; i < skinnedMeshRenderer.Length; i++)
                {
                    skinnedMeshRenderer[i].material.color = Color.white;
                }
            }
            else
                NowHiting += Time.deltaTime;
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
