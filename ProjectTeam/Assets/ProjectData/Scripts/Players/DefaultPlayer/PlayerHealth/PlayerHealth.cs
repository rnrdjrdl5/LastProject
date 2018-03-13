using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerHealth : Photon.PunBehaviour, IPunObservable
{

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {

        SyncHealth();

        isDead();

        

    }

    private void FixedUpdate()
    {

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        SendHealthData(stream);

        RecvHealthData(stream);
    }



    


}
