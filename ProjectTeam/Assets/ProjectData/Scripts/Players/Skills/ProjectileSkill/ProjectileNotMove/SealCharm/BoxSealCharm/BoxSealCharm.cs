using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoxSealCharm : DefaultDefaultProjectileNotMove, IPunObservable
{

    // Update is called once per frame
    override protected void Update()
    {

        base.Update();
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
