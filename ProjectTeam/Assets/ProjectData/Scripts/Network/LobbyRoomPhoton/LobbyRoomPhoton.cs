using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LobbyRoomPhoton : Photon.PunBehaviour {

    /**** public ****/

    public GameObject LobbyCanvas;                    // 게임시작 타이틀화면
    public GameObject WaitingRoomCanvas;                // 게임 대기실 , 메치메이킹 용도
    public GameObject RoomCanvas;                       // 게임방
    public GameObject RoomStartPanel;                   // 게임방 스타트 버튼

    public GameObject[] RoomPlayerListPanel;                   // 게임 방 플레이어 리스트

   
    public enum EnumCanvasState { LOBBY, WAITINGROOM , ROOM }            // 현재 방 상태   PSM으로 관리해도 될 것 같음.
    private EnumCanvasState CanvasStateType;                         // 현재 방 상태

    /**** private ****/
    private Button RoomStartButton;



    public string Version;              // 게임 버전

    // Use this for initialization
    private void Awake()
    {
        // 방 상태 로비로 결정
        CanvasStateType = EnumCanvasState.LOBBY;

        // 포톤 연결
        PhotonNetwork.ConnectUsingSettings(Version);

        // 스타트 버튼 받기
        RoomStartButton = RoomStartPanel.GetComponent<Button>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        
        // 게임방 상태일 경우
        if(CanvasStateType == EnumCanvasState.ROOM)
        {

            // Star버튼 활성화
            if(PhotonNetwork.isMasterClient)
            {

                //StartPanel 허용한다.
                RoomStartPanel.GetComponent<Button>().interactable = true;
            }
            else
            {

                // 일반 클라이언트 인 경우
                //StartPanel 해제한다.
                RoomStartPanel.GetComponent<Button>().interactable = false;
            }

            for (int i = 0; i < RoomPlayerListPanel.Length; i++)
            {
                
                // 유저 수 만큼 패널 상태 정하기
                if (i < PhotonNetwork.playerList.Length)
                {
                    Debug.Log("Player List " + i + " : " + PhotonNetwork.playerList[i].ID);
                    // 패널 갱신
                    RoomPlayerListPanel[i].GetComponent<Image>().color = Color.red;
                }
                else
                    RoomPlayerListPanel[i].GetComponent<Image>().color = Color.blue;
            }
        }
		
	}



    /**** Photon 함수 ****/

    public override void OnJoinedLobby()
    {
        Debug.Log("로비입장");
        base.OnJoinedLobby();

        // 대기실 캔버스 선택
        ChangeCanvas(WaitingRoomCanvas);

        // 대기실 상태로 변경
        CanvasStateType = EnumCanvasState.WAITINGROOM;

        // 랜덤 입장
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {

        // 방을 직접 만듬
        PhotonNetwork.CreateRoom("Room01");
    }

    public override void OnJoinedRoom()
    {
        // 룸 캔버스 선택
        ChangeCanvas(RoomCanvas);


        // 게임방 상태로 변경
        CanvasStateType = EnumCanvasState.ROOM;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        photonView.RPC("RPCSend", PhotonTargets.All);
    }


    /**** UI 이벤트 ****/

    /**** Lobby 이벤트 ****/
    public void ClickJoinButton()
    {

        // 방에 입장한다.
        PhotonNetwork.JoinLobby();


    }
    
    public void ClickRoomExit()
    {

        // 방에서 퇴장한다.
        Debug.Log("룸에서 나간다.;");
        PhotonNetwork.LeaveRoom();
        ChangeCanvas(LobbyCanvas);
        
    }

    public void ClickRoomStart()
    {
        photonView.RPC("RPCLoadLevel", PhotonTargets.All);
    }

    public void ClickExitGame()
    {
        Application.Quit();
    }


    /**** 함수 ****/
    void ChangeCanvas(GameObject NextCanvas)
    {
        LobbyCanvas.SetActive(false);
        WaitingRoomCanvas.SetActive(false);
        RoomCanvas.SetActive(false);

        NextCanvas.SetActive(true);
    }

    /**** RPC ****/
    [PunRPC]
    void RPCLoadLevel()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        //SceneManager.LoadScene(1);
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    void RPCSend()
    {
        Debug.Log("잘받았어;");
    }


}
