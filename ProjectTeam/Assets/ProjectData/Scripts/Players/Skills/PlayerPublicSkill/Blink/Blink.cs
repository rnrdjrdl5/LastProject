using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Blink : Photon.PunBehaviour, IPunObservable
{

    // Use this for initialization
    private void Awake()
    {
        
    }


    void Start () {
        SetPlayerCamera();
    }
	
	// Update is called once per frame
	void Update () {

        PlayerBlinkFromMouse();

        UpdateBlinkPosition();

        UseBlink();

	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

    }
}
