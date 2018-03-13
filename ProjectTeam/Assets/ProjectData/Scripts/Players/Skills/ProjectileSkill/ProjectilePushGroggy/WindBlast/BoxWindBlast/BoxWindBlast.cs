using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxWindBlast : DefaultProjectilePushGroggy, IPunObservable
{
    override protected void Update()
    {
        //UseWindBlast();
        base.Update();
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}