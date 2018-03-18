
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhotonManager : Photon.PunBehaviour
{

    private bool isJoinRoom = false;

    string Version = "test";

    public GameObject CurrentPlayer;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(Version);
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
                Debug.Log("a");
                if (CurrentPlayer != null)
                {
                    PhotonNetwork.Destroy(CurrentPlayer);
                }
                CurrentPlayer = PhotonNetwork.Instantiate("12Rion", Vector3.one * 3, Quaternion.identity, 0);
            }



            else if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("a");
                if (CurrentPlayer != null)
                {
                    PhotonNetwork.Destroy(CurrentPlayer);
                }
                CurrentPlayer = PhotonNetwork.Instantiate("12Box", Vector3.one * 3, Quaternion.identity, 0);
            }
        }
        


	}
}
