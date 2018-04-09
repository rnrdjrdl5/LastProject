using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/********
 *  구현방법
 *  CustomProperty의 해쉬값을 이용해서 각종 로딩 여부 파악 ( 코루틴 사용 ) 
 *  
 * 
 * ******/
public class PhotonManager : Photon.PunBehaviour{

    /**** public ****/
    public float timerNumber = 100.0f;              // 플레이 타이머


    /**** Private ****/
    private UIManager uIManager;                // UI 매니저

    IEnumerator CoroCheckGameStart;             // 게임시작체크 코루틴
    IEnumerator CoroCheckCreatePlayer;             // 플레이어생성체크 코루틴
    IEnumerator CoroCheckGameFinish;                // 게임종료체크 코루틴
    IEnumerator CoroTimer;                 // 게임 플레이 타이머 코루틴
    IEnumerator CoroGameStartRunTimer;             // 게임 달리기 전 시작 타이머
    IEnumerator CoroStartReStartTimer;              // 게임 재시작 타이머 

    private GameObject CurrentPlayer;               // 사용자 플레이어 포톤매니저에서 등록
    private PlayerCamera playerCamera;              // 플레이어 카메라

    private List<GameObject> AllPlayers;            // 모든 플레이어  각자등록


    /**** 접근자 ****/

    public void SetCurrentPlayer(GameObject Go)
    {
        CurrentPlayer = Go;
    }
    public void AddAllPlayer(GameObject Go) {
        AllPlayers.Add(Go);
    }

    public GameObject GetCurrentPlayer() {
        return CurrentPlayer;
    }


    /**** 유니티 함수 ****/

    private void Awake()
    {
        // 코루틴들 지정
        CoroCheckGameStart = CheckGameStart();
        CoroCheckCreatePlayer = CheckCreatePlayer();
        CoroCheckGameFinish = CheckGameFinish();
        CoroTimer = Timer();
        CoroGameStartRunTimer = GameStartRunTimer();
        CoroStartReStartTimer = StartReStartTimer();

        // 모든 플레이어 담을 오브젝트 생성
        AllPlayers = new List<GameObject>();

        // UIManager 찾기
        uIManager = GetComponent<UIManager>();

        // 카메라 찾기
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
    }

    private void Start()
    {

        // 플레이어 위치 씬 변경
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable { { "Scene", "InGame" } };
        PhotonNetwork.player.SetCustomProperties(ht);

        StartCoroutine(CoroCheckGameStart);
    }

    private void Update()
    {

    }

    /**** 함수 ****/
    bool CheckEndTimer()
    {
        if (timerNumber <= 0)
            return true;
        else
            return false;
    }

    // 모든 쥐가죽었는지 판단
    bool CheckMouseAllDead()
    {
        bool isFinish = true;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            string PlayerType = (string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"];

            if (PlayerType == "Mouse")
            {
                isFinish = false;
            }
        }

        return isFinish;
    }

    // 모든 상호작용 엎었는가
    bool CheckAllBreak()
    {
        return false;
    }

    // 게임이 끝났을 때 사용
    void ExecuteResult()
    {
        //ResultUI.SetActive(true);
    }

    /**** 코루틴 ****/

    // 로딩 완료됐는지 판단한다.
    IEnumerator CheckGameStart()
    {
        
        // 루프
        while (true)
        {
            bool isInGame = true;

            // 로딩 안된 클라이언트 찾기
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if ((string)PhotonNetwork.playerList[i].CustomProperties["Scene"] != "InGame")
                {
                    isInGame = false;
                }

                else
                {
                    Debug.Log(" 모든 플레이어 로딩 대기중... ");
                }
            }

            // 모두 로딩 완료
            if (isInGame)
            {
                StartCoroutine(CoroGameStartRunTimer);

                yield break;
        
                
            }

            yield return null;
        }
        
    }

    // 카운트 시작
    IEnumerator GameStartRunTimer()
    {
        int StartRunTimer = 3;
        uIManager.SetStartRunUI(true);
        uIManager.StartRunTextText.text = StartRunTimer.ToString();


        while (true)
        {

            uIManager.StartRunTextText.text = StartRunTimer.ToString();
            yield return new WaitForSeconds(1.0f);

            StartRunTimer -= 1;
            if (StartRunTimer == 0)
            {
                uIManager.SetStartRunUI(false);

                StartCoroutine(CoroCheckCreatePlayer);

                yield break;
            }
        }
    }

    // 플레이어 생성 완료됐는지 판단한다.
    IEnumerator CheckCreatePlayer()
    {

        // 서버 ++ ) 고양이 쥐 지정, 생성
        if (PhotonNetwork.isMasterClient)
        {


            int BossPlayer = -1;

            // 랜덤 플레이어 찾기
            while (true)
            {
                BossPlayer = Random.Range(0, PhotonNetwork.playerList.Length);

                if ((bool)PhotonNetwork.playerList[BossPlayer].CustomProperties["UseBoss"] == false)
                {
                    for(int i = 0; i < PhotonNetwork.playerList.Length; i++)
                    {
                        Debug.Log(i+" " + (bool)PhotonNetwork.playerList[i].CustomProperties["UseBoss"]);
                    }
                    break;
                }
            }

            // 해쉬 생성
            ExitGames.Client.Photon.Hashtable CatHash = new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Cat" } };
            ExitGames.Client.Photon.Hashtable MouseHash = new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Mouse" } };


            // 해쉬 대입
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if (BossPlayer == i)
                {
                    PhotonNetwork.playerList[i].SetCustomProperties(CatHash);
                }
                else
                {
                    PhotonNetwork.playerList[i].SetCustomProperties(MouseHash);
                }
            }
            // 플레이어 생성
            photonView.RPC("RPCCreatePlayer", PhotonTargets.All);




        }

        while (true)
        {
            bool isCreate = true;

            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                string CreatePlayerState = (string)PhotonNetwork.playerList[i].CustomProperties["Offset"];

                if (CreatePlayerState != "CreateComplete")
                {
                    isCreate = false;
                }
                else
                {
                    Debug.Log("  플레이어 생성 대기 .. ");
                }
            }

            if (isCreate)
            {
                Debug.Log("플레이어 생성 완료!");


                // 1. 타이머 UI 보여주기
                uIManager.SetUICanvas(true);

                // 게임 종료조건 코루틴 생성
                StartCoroutine(CoroCheckGameFinish);

                // 타이머 시작
                StartCoroutine(CoroTimer);
                yield break;
            }


            yield return null;
        }
    }

    // 게임 끝나는 지 판단
    IEnumerator CheckGameFinish()
    {

        // 게임 끝났는지 파악
        while (true)
        {
            if(CheckMouseAllDead() || CheckEndTimer() || CheckAllBreak())
            {

                // 모든 플레이어 삭제
                if (PhotonNetwork.isMasterClient)
                {
                    int Type = 0;

                    if (CheckAllBreak())
                        Type = 0;
                    else if (CheckMouseAllDead())
                        Type = 1;
                    else if (CheckEndTimer())
                        Type = 2;

                    photonView.RPC("RPCDeleteResult", PhotonTargets.All,Type);
                }

                for(int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if ((bool)PhotonNetwork.playerList[i].CustomProperties["UseBoss"] == false)
                    {
                        StartCoroutine(CoroStartReStartTimer);
                        yield break;
                    }
                }

                PhotonNetwork.LeaveRoom();
                // 모두 한번씩 보스를 했다. 그러므로 다 나가라.



            }

            yield return null;
        }
    }

    public override void OnLeftRoom()
    {   //none 안가침 
        Cursor.lockState = CursorLockMode.Confined;

        Cursor.visible = true;

        SceneManager.LoadScene(0);
        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("1");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("2");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("3");
    }
    // 게임 3초뒤에 재로딩시작
    IEnumerator StartReStartTimer()
    {
        float RestartTimer = 5.0f;
        while (true)
        {
            yield return null;
            RestartTimer -= Time.deltaTime;

            if(RestartTimer <= 0)
            {
                PhotonNetwork.player.SetCustomProperties(
                    new ExitGames.Client.Photon.Hashtable { { "Offset", "RestartComplate" } });

                Debug.Log(" 재시작 로딩 완료.");
            }


            bool isComplate = true;
            for(int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {

                if ((string)PhotonNetwork.playerList[i].CustomProperties["Offset"] != "RestartComplate")
                {
                    isComplate = false;
                }
            }

            if (isComplate)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.LoadLevel(2);
                }
                yield break;

            }
        }
    }

    // 타이머 코루틴
    IEnumerator Timer()
    {
        uIManager.LimitTimeTextText.text = timerNumber.ToString();

        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            uIManager.LimitTimeTextText.text = timerNumber.ToString();

            timerNumber -= 1.0f;
            if (timerNumber <= 0.0f)
            {
                uIManager.LimitTimeTextText.text = timerNumber.ToString();
                yield break;
            }
        }
    
    }

    /**** RPC ****/

    // 플레이어 생성
    [PunRPC]
    void RPCCreatePlayer()
    {
        Debug.Log("aa + " + PhotonNetwork.player.ID);


        // 플레이어 생성
        string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

        // 고양이는 추가로 고양이가 될 수 없도록 해쉬값 생성
        if (PlayerType == "Cat")
        {
            CurrentPlayer = PhotonNetwork.Instantiate("NewCatBoss", Vector3.zero, Quaternion.identity, 0);
            PhotonNetwork.player.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "UseBoss", true } });
        }

        else if (PlayerType == "Mouse")
            CurrentPlayer = PhotonNetwork.Instantiate("MouseRunner", Vector3.zero, Quaternion.identity, 0);


        // 생성했다는 의미로 오프셋 사용
        PhotonNetwork.player.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable{{"Offset", "CreateComplete" }});


        
    }

    // 플레이어 삭제
    [PunRPC]
    void RPCDeleteResult(int i)
    {
        // 카메라 스폰 취소
        playerCamera.isPlayerSpawn = false;


        // 플레이어 삭제 처리
        if(CurrentPlayer != null)
            PhotonNetwork.Destroy(CurrentPlayer);

        // 플레이어 UICanvas 끄기
        uIManager.SetUICanvas(false);

        // 플레이어 Result UI 설정
        uIManager.SetResultUI(true, (UIManager.ResultType)i);

        Debug.Log("끝");
    }




}




