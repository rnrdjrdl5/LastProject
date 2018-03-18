using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : Photon.PunBehaviour {

    
    public GameObject PlayerObject; // 플레이어 객체의 함수를 사용하기 위해 사용. 생성 시 할당
    public GameObject UICanvas; // UI 삭제용

    public GameObject ResultPanel; // UI 생성, 결과창 용.
    public GameObject ResultObject; // 확인용
    /// </summary>



    public string Version = "ffffqqqq";

    public bool isGameStart = false;

    private float TimeCount = 0;
    public void SetTimeCount(float TC) { TimeCount = TC; }
    public float GetTimeCount() { return TimeCount; }

    public float PlayerTime = 100.0f;

    private int BossPlayerCount;
    public void SetBossPlayerCount(int BPC) { BossPlayerCount = BPC; }
    public int GetBossPlayerCount() { return BossPlayerCount; }

    private int TeamsPlayerCount;
    public void SetTeamsPlayerCount(int TPC) { TeamsPlayerCount = TPC; }
    public int GetTeamsPlayerCount() { return TeamsPlayerCount; }

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(Version);
        TeamsPlayerCount = 0;
        BossPlayerCount = 0;
        
    }

    // Use this for initialization
    void Start () {

    }
	
    

	// Update is called once per frame
	void Update () {
        //Debug.Log(PhotonNetwork.isMasterClient);
        if (PhotonNetwork.isMasterClient)
        {
           
            if (Input.GetKeyDown(KeyCode.T) && !isGameStart)
            {
                
                if (PhotonNetwork.room.PlayerCount >= 1)
                {
                    int BossPlayerNumber = Random.Range(0, PhotonNetwork.room.PlayerCount);

                    ExitGames.Client.Photon.Hashtable isBossPlayer = new ExitGames.Client.Photon.Hashtable { { "Boss", "True" } };
                    ExitGames.Client.Photon.Hashtable isNotBossPlayer = new ExitGames.Client.Photon.Hashtable { { "Boss", "False" } };

                    foreach (PhotonPlayer PP in PhotonNetwork.playerList)
                    {
                        if(PP == PhotonNetwork.playerList[BossPlayerNumber])
                        {
                            PP.SetCustomProperties(isBossPlayer);
                        }
                        else
                        {
                            PP.SetCustomProperties(isNotBossPlayer);
                        }
                        // 마스터에서 다른애들로 변경불가인거같네.
                        //PP.NickName = "Player 1";
                    }
                    

                    photonView.RPC("CallSetisGameStart", PhotonTargets.All, true);
                    photonView.RPC("CreateGamePlayer", PhotonTargets.All,BossPlayerNumber);
                    photonView.RPC("SetCount", PhotonTargets.All , PlayerTime);

                    photonView.RPC("RPCDestroyResultUI", PhotonTargets.All);

                  /*  if (ResultObject!= null)
                    {
                        Destroy(ResultObject);
                    }*/



                }
                
            }
            if(Input.GetKeyDown(KeyCode.Delete))
            {


                photonView.RPC("DestroyAllPlayer", PhotonTargets.All);
                photonView.RPC("CallSetisGameStart", PhotonTargets.All, false);
                photonView.RPC("ResetBossTeams", PhotonTargets.All);
            }
        }

        // 숫자 카운트
        if(TimeCount > 0 )
        {
            TimeCount -= Time.deltaTime;
            if (TimeCount <= 0)
            {
                TimeCount = 0;
                DestroyPlayers();
            }
        }

        if (PhotonNetwork.isMasterClient)
        {
            if (TimeCount > 0 && TimeCount < 95)
            {
                if(TeamsPlayerCount <= 0)
                {
                    photonView.RPC("DestroyAllPlayer", PhotonTargets.All);
                    photonView.RPC("CallSetisGameStart", PhotonTargets.All, false);
                    photonView.RPC("ResetBossTeams", PhotonTargets.All);


                    //photonView.RPC("RPCInstantiateResult", PhotonTargets.All);
                    StartCoroutine("WaitTime");

                    //photonView.RPC("SetCount", PhotonTargets.All, 0);
                    //  isGameStart = false;

                }
                else if(BossPlayerCount <= 0)
                {
                    photonView.RPC("DestroyAllPlayer", PhotonTargets.All);
                    photonView.RPC("CallSetisGameStart", PhotonTargets.All, false);
                    photonView.RPC("ResetBossTeams", PhotonTargets.All);


                    StartCoroutine("WaitTime");

                    //photonView.RPC("RPCInstantiateResult", PhotonTargets.All);

                    // photonView.RPC("SetCount", PhotonTargets.All, 0);
                    //  isGameStart = false;
                }

            }
        }

        
        
	}

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1.0f);
        photonView.RPC("RPCInstantiateResult", PhotonTargets.All);
        yield return null;
    }

    [PunRPC]
    void ResetBossTeams()
    {
        BossPlayerCount = 0;
        TeamsPlayerCount = 0;
    }

    [PunRPC]
    void CallSetisGameStart(bool GS)
    {
        isGameStart = GS;
    }



    [PunRPC]
    void DestroyAllPlayer()
    {
        GameObject[] Gs=  GameObject.FindGameObjectsWithTag("Teams");

        foreach(GameObject g in Gs)
        {
          if(g.GetComponent<PhotonView>().isMine)
            {
                GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().isPlayerSpawn = false;

                PhotonNetwork.Destroy(g);

                Destroy(g.GetComponent<PlayerUI>().GetUIObject());
            }
        }


        Gs = GameObject.FindGameObjectsWithTag("Boss");

        foreach (GameObject g in Gs)
        {
            if (g.GetComponent<PhotonView>().isMine)
            {
                GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().isPlayerSpawn = false;

                PhotonNetwork.Destroy(g);

                Destroy(g.GetComponent<PlayerUI>().GetUIObject());
            }
        }

        TimeCount = 0;
    }

    public void CallDestroyAllPlayer()
    {
        photonView.RPC("DestroyAllPlayer", PhotonTargets.All);
    }








    void DestroyPlayers()
    {
        Destroy(UICanvas);
        GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().isPlayerSpawn = false;
        PlayerObject.GetComponent<PlayerHealth>().DestroyPlayer();
        isGameStart = false;
    }



    public override void OnJoinedLobby()

    {
        // 일단은 자동으로 로비에 가게 되어있음.
        PhotonNetwork.JoinRandomRoom();

        // 해당 함수로 랜덤이 아닌 지정해서 갈 수 있음.

        //PhotonNetwork.JoinRoom

        // 실패하면 콜백으로  OnPhotonJoinRoomFailed () 호출됨. 구현해줘야ㅐ함.
        // 실패이유는 방이 꽉찼다던지 방이 없다던지 등.

    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {

        // expectedUsers 멤버변수 : 예약석을 위한 도구, 친구 자리 마련 등.
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        //PhotonNetwork.CreateRoom("TestRoom");
        // 만들 때 룸 정보를 설정할 수 있고 해시테이블을 이용해서 속성 설정가능
        // 룸을 찾을 때 해시테이블을 이용해서 확인 가능.
        PhotonNetwork.CreateRoom("INFY10Room");
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("룸입장성공");

        PhotonNetwork.player.NickName = (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1];
        Debug.Log(PhotonNetwork.player.NickName);

        gameObject.AddComponent<PhotonView>();



        //Debug.Log("테스트 id, 동일여부 " + PhotonNetwork.player.ID); 다르게나옴

        /* if (PhotonNetwork.isMasterClient)
         {
             PhotonNetwork.Instantiate("12Rion", new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2)), Quaternion.identity, 0);
         }

         else
         {
             PhotonNetwork.Instantiate("12Tiger", new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2)), Quaternion.identity, 0);
         }*/


        //Debug.Log(PhotonNetwork.room.PlayerCount + "명");



        // 액터는 기존에 있던 플레이어가 나가도 채워넣지 않고 다음 액터id를 사용함.
        // 각 액터는 클라이언트당 하나씩 부여됨.
        //Debug.Log(PhotonNetwork.player.ID + "액터 id");



        // PhotonPlayer는 클라이언트당 하나를 가진다.
        // NickName은 플레이어의 이름이다.
        //  nickname playerName은 같다. 



        // 각 PhotonPlayer는 Custom Properties를 가지고 있다. 
        // 해당 해쉬를 참조해서 상태를 파악한다. 
        // 설정은 PhotonNetwork.SetPlayerCustomProperties  으로 .

        // 키와 값을 판단해서 사용. 해쉬 기억안나? 나지 ? 
        //PhotonNetwork.player.CustomProperties




        /*
        PhotonNetwork.player.NickName = "나야나" + PhotonNetwork.player.ID;

        Debug.Log(PhotonNetwork.playerName);


        Debug.Log(PhotonNetwork.playerList[0].NickName);
        */


        //Debug.Log(PhotonNetwork.player.NickName + "닉네임 id");

        /*
        Room ro = PhotonNetwork.room;
        Debug.Log(ro.PlayerCount);
        Debug.Log(ro.MaxPlayers);
        */

        // PhotonNetwork.player.AddScore(1);
        // 포톤에서 기본적으로 제공해주는 저장시스템.


        // 로비가 아닌 방 내부에서 사용가능함.
        // 방의 플레이어 수랑, 최대 플레이어 를 알 수 있음.

        //자신 player정보 받아오는법
        //PhotonNetwork.player

        // 연결된 모든 player 정보 받아오는 법.
        //PhotonNetwork.playerList



    }

    public override void OnReceivedRoomListUpdate()
    {
        // 콜백함수, 방들 리스트 를 받기 위해 사용한다.
        // 아래의 함수로 받아올 수 있다.
        // 리스트를 띄울 수 있다는 이야기.

        // 응용하면, 해당 리스트를 정해서 정보를 얻은다음에 ( 해쉬값 ) 

        // random으로 돌리면서, 해쉬값과 맞는 걸 찾으면!
        // 선택해서 방을 들어갈 수 있음.

        //RoomInfo[] r = PhotonNetwork.GetRoomList();
        
    
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방나감");
    }

    // 설명을 위한 것, 아래다가 적는다. 
    /*
    public void comment()
    {
        // expectedMaxPlayer:  최대 플레이어 설정, 사용자가 정하기.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = expectedMaxPlayers;


    //1. HashTable 생성 후 정보값 입력 
     //2. 해쉬 키에 대한 정보를 주기 위해서 customRoomPropertiesForLobby에 넣어준다.
     // 방 설정부분. 다른 멤버변수도 있음. 공개가능이라던지 최대인원이라던지
        roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "C0", 1 } };
        roomOptions.customRoomPropertiesForLobby = new string[] { "C0" };


    // sql 형태 로비. 반드시 
    // sql처럼 질의형 을 넣어주기 위해 설정, LobbyType.SqlLobby는 질문형 의라는 의미. LobbyType.Default 는 아무 설정 없이 랜덤 매칭용 
    // 방이름 같은 느낌.
        TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
        PhotonNetwork.CreateRoom(roomName, roomOptions, sqlLobby);


    // sql 을 설정. 방이름 같은 느낌.
    TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);   

    // 방 설정에 들어있는 해쉬를 파악하기 위해 사용. 해쉬 키값은 CO이 되고, 데이터는 0 이 된다. 
    string sqlLobbyFilter = "C0 = 0";   // find a game with mode 0

    // MatchmakingMode는 정보와 일차하는 방이 여러개 있을 때 방을 어떤 방법으로 채울지  순서대로 채울지 평균적으로 채울지 결정. 
    // 여기서는 변수로 사용했지만 사실 MatchmakingMde.종류 로 사용해도 된다.
    PhotonNetwork.JoinRandomRoom(null, expectedMaxPlayers, matchmakingMode, sqlLobby, sqlLobbyFilter);
    }*/

    [PunRPC]
    void CreateGamePlayer(int BossPlayerNumber)
    {

        // 언박싱,
        // 값형태 : 스택 ( int , string 등 ) 
        // 참조형태 : 힙 ( object .  ) 
        // 힙 > 스택 : 언박싱
        // 스택 > 힙 : 박싱
        // 성능에 안좋음. 하지만 써야함.
        if (PhotonNetwork.player.CustomProperties["Boss"] as string == "True")
        {

            PlayerObject = PhotonNetwork.Instantiate("12Rion", new Vector3(Random.Range(-30, 25), 0, Random.Range(-30, 25)), Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);

            photonView.RPC("IncreaseBossCount", PhotonTargets.All, 1);
        }

        else if(PhotonNetwork.player.CustomProperties["Boss"] as string == "False")
        {
            PlayerObject = PhotonNetwork.Instantiate("12Box", new Vector3(Random.Range(-30, 25), 0, Random.Range(-30, 25)), Quaternion.identity, 0);
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);

            photonView.RPC("IncreaseTeamsCount", PhotonTargets.All, 1);
        }

        
    }

    [PunRPC]
    void SetCount(float time)
    {
        TimeCount = time;
    }


    // 에러 있음. 0이 아닌 -1 이하로 내려감. 아마 호출이 더 많이되는거같은데?
    [PunRPC]
    void IncreaseBossCount(int BC)
    {
        BossPlayerCount += BC;
    }
    public void CallSetBossCount(int BC)
    {
        photonView.RPC("IncreaseBossCount", PhotonTargets.All, BC);
    }

    [PunRPC]
    void IncreaseTeamsCount(int TC)
    {
        TeamsPlayerCount += TC;
    }

    public void CallSetTeamsCount(int BC)
    {
        photonView.RPC("IncreaseTeamsCount", PhotonTargets.All, BC);
    }

    [PunRPC]
    void RPCInstantiateResult()
    {
        ResultObject = Instantiate(ResultPanel);
    }

    [PunRPC]
    void RPCDestroyResultUI()
    {
        if (ResultObject != null)
        {
            Destroy(ResultObject);
        }
    }

   
}




