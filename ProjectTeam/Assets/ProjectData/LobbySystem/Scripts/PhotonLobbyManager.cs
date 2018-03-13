using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobbyManager : Photon.PunBehaviour {

    string Version = "a";

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(Version);
        PhotonNetwork.autoJoinLobby = false;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(" 로비에 입장했다. ");
    }

}
