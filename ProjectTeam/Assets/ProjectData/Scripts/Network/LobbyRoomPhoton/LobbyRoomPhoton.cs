using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LobbyRoomPhoton : Photon.PunBehaviour {




    /**** public ****/

    public enum EnumCanvasState { LOBBY, WAITINGROOM, ROOM }            // 현재 방 상태   PSM으로 관리해도 될 것 같음.

    public string Version;              // 게임 버전

    public GameObject LobbyCanvas;                    // 게임시작 타이틀화면
    public GameObject WaitingRoomCanvas;                // 게임 대기실 , 메치메이킹 용도
    public GameObject RoomCanvas;                       // 게임방
    public GameObject RoomStartPanel;                   // 게임방 스타트 버튼  

    public XMLManager xmlManager;               // XML 전용

    [System.Serializable]
    public struct RoomPlayerObject
    {
        public GameObject RoomPlayerListPanel;
        public Image RoomPlayerListPanelImage;
        public Text PlayerName;
        public Image MasterClientImage;
    }
    public RoomPlayerObject[] roomPlayerObject;


    /**** private ****/

    private EnumCanvasState CanvasStateType;                         // 현재 방 상태

    private struct RoomPlayerData
    {
        public string PlayerName;
        public int PlayerID;
        public bool isMasterClient;
    }
    private List<RoomPlayerData> roomPlayerData;

    private Button RoomStartButton;

    private IEnumerator CoroFindPlayerName;



    /**** 유니티 함수 ****/

    private void Awake()
    {

        // 방 상태 로비로 결정
        CanvasStateType = EnumCanvasState.LOBBY;

        // 포톤 연결
        PhotonNetwork.ConnectUsingSettings(Version);

        // 스타트 버튼 받기
        RoomStartButton = RoomStartPanel.GetComponent<Button>();

        // 플레이어 리스트 초기화
        roomPlayerData = new List<RoomPlayerData>();

        // 접속자들 한번에 로딩처리
        PhotonNetwork.automaticallySyncScene = true;

        

    }

    private void Update()
    {
        if (CanvasStateType == EnumCanvasState.ROOM)
        {
            roomPlayerData.Clear();
            InitPlayerList();
            DrawRoomState();
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

        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 6
        };

        PhotonNetwork.CreateRoom("Catching" + Random.Range(0,1000).ToString(),ro,TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // 룸 캔버스 선택
        ChangeCanvas(RoomCanvas);

        // 게임방 상태로 변경
        CanvasStateType = EnumCanvasState.ROOM;

        // 이름 랜덤 생성
        CreateRandomID();

        // 플레이어 정보 생성
        InitPlayerList();

        // 그리기
        DrawRoomState();

        // 씬을 위해서 해쉬 생성
        ExitGames.Client.Photon.Hashtable PlayerSceneState = new ExitGames.Client.Photon.Hashtable
        {
            { "Scene", "Room" }
        };

        ExitGames.Client.Photon.Hashtable PlayerLoadingState = new ExitGames.Client.Photon.Hashtable
        {
            { "Offset","NULL" }
        };

        PhotonNetwork.player.SetCustomProperties(PlayerSceneState);
        PhotonNetwork.player.SetCustomProperties(PlayerLoadingState);

    }





    /**** UI 이벤트 ****/

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
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        PhotonNetwork.LoadLevel("Loading");
    }

    public void ClickExitGame()
    {
        Application.Quit();
    }


    /**** 함수 ****/

    // 캔버스 교체
    void ChangeCanvas(GameObject NextCanvas)
    {
        LobbyCanvas.SetActive(false);
        WaitingRoomCanvas.SetActive(false);
        RoomCanvas.SetActive(false);

        NextCanvas.SetActive(true);
    }

    // 스타트 버튼 활성화 체크
    void CheckStartButton()
    {
        if (PhotonNetwork.isMasterClient)
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
    }

    // 플레이어 리스트 초기화 후 정렬
    void InitPlayerList()
    {
       
        // 플레이어 정보 생성
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            // 플레이어 데이터 생성 후 삽입
            CreateAddPlayerData(PhotonNetwork.playerList[i]);
        }

        // 플레이어 데이터 정렬
        SortPlayerData();
    }

    // Room UI 그리기 
    void DrawRoomState()
    {
        // 스타트버튼 활성화
        CheckStartButton();


        // 패널  수 
        for (int i = 0; i < roomPlayerObject.Length; i++)
        {

            // 유저 접속 패널
            if (i < PhotonNetwork.playerList.Length)
            {


                roomPlayerObject[i].RoomPlayerListPanelImage.color = Color.red;

                roomPlayerObject[i].PlayerName.text = roomPlayerData[i].PlayerName;


                // 접속자 중에 마스터만 .
                roomPlayerObject[i].MasterClientImage.color =
                    roomPlayerData[i].isMasterClient == true ?
                    Color.white : Color.clear;
            }


            // 유저가 접속 안한 패널
            else
            {
                roomPlayerObject[i].RoomPlayerListPanelImage.color = Color.blue;

                roomPlayerObject[i].PlayerName.text = "";

                roomPlayerObject[i].MasterClientImage.color = Color.clear;
            }

        }

    }

    // 플레이어 데이터 정렬
    void SortPlayerData()
    {
        // 람다식 ,무명 메소드를 간소화
        roomPlayerData.Sort(
            (RoomPlayerData rp1, RoomPlayerData rp2) =>
            {
                if (rp1.PlayerID > rp2.PlayerID)
                    return 1;
                else if (rp1.PlayerID < rp2.PlayerID)
                    return -1;
                return 0;
            });
    }

    // 플레이어 데이터 생성 후 삽입
    void CreateAddPlayerData(PhotonPlayer pp)
    {
        // 정보 생성
        RoomPlayerData rp = new RoomPlayerData
        {
            PlayerName = pp.NickName,
            PlayerID = pp.ID
        };

        rp.isMasterClient = pp.IsMasterClient ? true : false;

        // 리스트에 삽입
        roomPlayerData.Add(rp);
    }

    // ID 생성
    void CreateRandomID()
    {
        // 플레이어 이름 정하기
        // (로딩 시간 고려해서 미리 로딩해야함)
        xmlManager = new XMLManager();
        List<string> Names = xmlManager.XmlRead();


        // 플레이어 이름 선택 
        while (true)
        {
            string RandomMyName = Names[Random.Range(0, Names.Count)];

            bool isFind = false;


            string SelectMyName = null;
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {

                // 없으면 자기 닉네임으로 선정
                if (PhotonNetwork.playerList[i].NickName != RandomMyName)
                {

                    if (!isFind)
                        SelectMyName = RandomMyName;
                    isFind = true;
                }

                // 하나라도 겹치면 제외
                else
                {
                    Names.Remove(RandomMyName);
                    isFind = false;
                    break;
                }
            }

            // 찾으면 나감.
            if (isFind)
            {
                PhotonNetwork.playerName = SelectMyName;
                break;
            }

        }

    }
}
