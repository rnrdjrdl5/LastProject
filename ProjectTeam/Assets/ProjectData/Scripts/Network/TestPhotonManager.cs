
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhotonManager : Photon.PunBehaviour
{

    public GameObject PlayerSpawnLocation;              // 플레이어 리스폰 자리 


    private PhotonView pv;              // 포톤뷰

    string Version = "tes34151522t1";

    public GameObject CurrentPlayer;        // 플레이어

    public GameObject PlayerCamera;         // 카메라
    private PlayerCamera playerCamera;

    public PointToLocation PTL;             // 레이용 스크립트


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
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom("testr34133oom1");
    }
    public override void OnJoinedRoom()
    {

    }

    // Use this for initialization
    void Start () {
		
	}


    string hitName = "X";
    float hitDistance = 0.0f;
    
    // Update is called once per frame
    void Update() {

        if (photonView.isMine)
        {
            // 생성
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (CurrentPlayer != null)
                {
                    Destroy(CurrentPlayer.GetComponent<PlayerUI>().GetUIObject());
                    PhotonNetwork.Destroy(CurrentPlayer);
                }

                CurrentPlayer = PhotonNetwork.Instantiate("NewCatBoss", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);
            }

            //생성
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("a");
                if (CurrentPlayer != null)
                {
                    Destroy(CurrentPlayer.GetComponent<PlayerUI>().GetUIObject());
                    PhotonNetwork.Destroy(CurrentPlayer);
                }

                CurrentPlayer = PhotonNetwork.Instantiate("MouseRunner", PlayerSpawnLocation.transform.position, Quaternion.identity, 0);
            }

            // 레이캐스트 발사
            if (Input.GetKeyDown(KeyCode.V))
            {

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

            
        }
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 400 * Screen.height / 800, 500, 20), "대상이름 : " + hitName);
        GUI.Label(new Rect(0, 440 * Screen.height / 800, 500, 20), "대상과 거리 : " + hitDistance);
    }

}
