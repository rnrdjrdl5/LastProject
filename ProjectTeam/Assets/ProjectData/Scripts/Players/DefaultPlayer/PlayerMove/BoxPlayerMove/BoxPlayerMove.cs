using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxPlayerMove : Photon.PunBehaviour, IPunObservable
{
    private void Awake()
    {
        SetAwake();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        PlayerTransform();

        PlayerMoveAnimation();
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        SendTransform(stream);

        RecvTransform(stream);


    }
}
