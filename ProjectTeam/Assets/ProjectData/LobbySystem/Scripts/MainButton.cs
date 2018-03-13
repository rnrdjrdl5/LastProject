using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButton : Photon.PunBehaviour {

    public Transform MyCanvas;
    public GameObject LobbyCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLobby()
    {
        while (MyCanvas.transform.parent != null)
        {
            MyCanvas = MyCanvas.transform.parent;
        }
        PhotonNetwork.JoinLobby();

        Instantiate(LobbyCanvas);
        Destroy(MyCanvas.gameObject);
    }
}
