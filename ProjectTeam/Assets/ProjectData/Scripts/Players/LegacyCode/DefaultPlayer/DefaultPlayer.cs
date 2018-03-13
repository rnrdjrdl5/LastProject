using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 public partial class DefaultPlayer : Photon.PunBehaviour, IPunObservable
{

    void Awake()
    {
        //SetAwake();
        OtherAwake();
    }

	// Use this for initialization
	void Start () {
       // SetUI();

        OtherStart();
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*
        PlayerTransform();

        PlayerMoveAnimation();

        SetSyncData();
        */
        //SyncMoveAnimation();

        OtherUpdate();
    }

    void FixedUpdate()
    {
       // KnockingBackPlayer();

        OtherFixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
       // OnDeadZone(other);

       // CheckTriggerObject(other);

        OtherOnTriggerEnter(other);
    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
          /*  stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);*/
          //  stream.SendNext(NowHealth);
        }
        else
        {
           /* RecvPosition = (Vector3)stream.ReceiveNext();
            RecvRotation = (Quaternion)stream.ReceiveNext();*/
          //  RecvHealth = (float)stream.ReceiveNext();

        }

        OtherOnPhotonSerializeView(stream, info);
    }



}
