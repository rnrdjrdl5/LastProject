
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhotonManager : Photon.PunBehaviour
{

    public GameObject PlayerSpawnLocation;              // 플레이어 리스폰 자리 


    private PhotonView pv;              // 포톤뷰

    string Version = "20180404t1";

    public GameObject CurrentPlayer;        // 플레이어

    public GameObject PlayerCamera;         // 카메라
    private PlayerCamera playerCamera;

    public PointToLocation PTL;             // 레이용 스크립트

    public UIManager uIManager;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(Version);

        pv = gameObject.GetComponent<PhotonView>();
        PTL = new PointToLocation();
        PTL.SetPlayerCamera(PlayerCamera);
        playerCamera = PlayerCamera.GetComponent<PlayerCamera>();
        PTL.SetcameraScript(playerCamera);


    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비입장");
        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom("20180413");
    }
    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable MouseScore = new ExitGames.Client.Photon.Hashtable
        {
            { "MouseScore",0}
        };

        ExitGames.Client.Photon.Hashtable CatScore = new ExitGames.Client.Photon.Hashtable
        {
            { "CatScore",0 }
        };


        PhotonNetwork.player.SetCustomProperties(MouseScore);
        PhotonNetwork.player.SetCustomProperties(CatScore);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {

    }


    // Use this for initialization
    void Start () {
        PhotonNetwork.isMessageQueueRunning = true;        

    }


    string hitName = "X";
    float hitDistance = 0.0f;

    public GameObject doq;
    // Update is called once per frame
    void Update() {
        
            // 생성
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("123");
                if (CurrentPlayer != null)
                {
                    PhotonNetwork.Destroy(CurrentPlayer);
                }

                CurrentPlayer = PhotonNetwork.Instantiate("Cat/CatBoss", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);

            uIManager = CurrentPlayer.GetComponent<UIManager>();
            uIManager.mPPanelScript.SetManaPoint(true);
            uIManager.hPPanelScript.SetHealthPoint(true);
            uIManager.SetAim(true);

            uIManager.Players.Add(PhotonNetwork.player);

        }

            //생성
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("a");
                if (CurrentPlayer != null)
                {
                    
                    PhotonNetwork.Destroy(CurrentPlayer);
                }

                CurrentPlayer = PhotonNetwork.Instantiate("Mouse/MouseRunner", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);

                uIManager = CurrentPlayer.GetComponent<UIManager>();
                uIManager.mPPanelScript.SetManaPoint(true);
               uIManager.hPPanelScript.SetHealthPoint(true);
               uIManager.SetAim(true);

            uIManager.Players.Add(PhotonNetwork.player);
        }

            // 레이캐스트 발사
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("발사");
                // 마우스 위치 받기
                Vector3 MouseVector3 = PTL.FindMouseCursorPosition(CurrentPlayer, PlayerCamera);

                RaycastHit hit;


                if (Physics.Raycast(PlayerCamera.transform.position + MouseVector3.normalized * playerCamera.CameraDistanceTriangle,
                    MouseVector3, out hit, 20,~(1<<LayerMask.NameToLayer("Floor") )))
                {

                    float distance = (hit.collider.transform.position - CurrentPlayer.transform.position).magnitude;

                    

                    Debug.DrawRay(PlayerCamera.transform.position + MouseVector3.normalized * playerCamera.CameraDistanceTriangle,
                    MouseVector3 * distance, Color.red, 1.0f);

                    
                    hitName = hit.collider.name;
                    hitDistance = distance;
                }
                else
                    hitName = "없음";

            }
            
            else if(Input.GetKeyDown(KeyCode.End))
            {
                photonView.RPC("RPCReScene", PhotonTargets.All);
            }

            
        
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 400 * Screen.height / 800, 500, 20), "대상이름 : " + hitName);
        GUI.Label(new Rect(0, 440 * Screen.height / 800, 500, 20), "대상과 거리 : " + hitDistance);
    }

    [PunRPC]
    void RPCReScene()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel(0);
    }

}
