
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhotonManager : Photon.PunBehaviour
{
    public GameObject PlayerSpawnLocation;

    public GameObject InteractionPrefab;
    public GameObject InteractionObject;

    private PhotonView pv;

    private bool isJoinRoom = false;

    string Version = "tes2t1";

    public GameObject CurrentPlayer;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(Version);

        pv = gameObject.GetComponent<PhotonView>();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom("testroom1");
    }
    public override void OnJoinedRoom()
    {
        isJoinRoom = true;
        Debug.Log("방접속완료");

        if (PhotonNetwork.isMasterClient)
        {
           // SpawnObject();
        }
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (photonView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (CurrentPlayer != null)
                {
                    PhotonNetwork.Destroy(CurrentPlayer);
                }
                //CurrentPlayer = PhotonNetwork.Instantiate("12Rion", Vector3.one * 3, Quaternion.identity, 0);
                //CurrentPlayer = PhotonNetwork.Instantiate("CatBoss", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);
                CurrentPlayer = PhotonNetwork.Instantiate("NewCatBoss", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);
            }



            else if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("a");
                if (CurrentPlayer != null)
                {
                    PhotonNetwork.Destroy(CurrentPlayer);
                }

                //CurrentPlayer = PhotonNetwork.Instantiate("12Box", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);
                CurrentPlayer = PhotonNetwork.Instantiate("MouseRunner", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);
            }



            if (PhotonNetwork.isMasterClient)
            {
                if (Input.GetKeyUp(KeyCode.End))
                {
                    ResetInteraction();
                }
            }
        }
        


	}


    void ResetInteraction()
    {
        DestroyObject();    
        SpawnObject();
    }

    void SpawnObject()
    {
        InteractionObject = PhotonNetwork.Instantiate("Interactions", Vector3.zero, Quaternion.identity, 0);
    }

    void DestroyObject()
    {
        PhotonNetwork.Destroy(InteractionObject);
    }
}
