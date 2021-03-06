﻿using System.Collections;
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
public class PhotonManager : Photon.PunBehaviour , IPunObservable
{
    // 싱글톤
    private static PhotonManager photonManager;
    public static PhotonManager GetInstance() { return photonManager; }



    // 드라마틱 카메라 모드 
    public GameObject DramaticCameraPrefab;
    private GameObject DramaticCameraObject;

    bool isObserverMode = true;


    delegate bool Condition();
    delegate void ConditionLoop();
    delegate int RPCActionType();

    delegate void ActionMine(params object[] obj);

    Condition condition;
    ConditionLoop conditionLoop;
    RPCActionType rPCActionType;

    ActionMine actionMine;

    /**** public ****/
    public int playTimerNumber = 5;              // 플레이 타이머
    public int MaxCatScore;                     // 쥐 최대 스코어

    public float PreScoreUITimer = 2.0f;
    public float NextRoundTimer = 2.0f;


    /***** 외부 설정 *****/


    public List<GameObject> AllPlayers;            // 플레이어들을 가리키는 변수


    public GameObject MouseLocation;
    public GameObject CatLocation;

    /**** Private ****/
    private UIManager uIManager;                // UI 매니저

    private float TimerValue;               // 대기시간 기다리는 용도


    IEnumerator IEnumCoro;

    private GameObject CurrentPlayer;               // 사용자 플레이어 포톤매니저에서 등록
    private PlayerCamera playerCamera;              // 플레이어 카메라
    private ObjectManager objectManager;            // 오브젝트 매니저
    
    

    /**** 접근자 ****/

    public void SetCurrentPlayer(GameObject Go)
    {
        CurrentPlayer = Go;
    }

    public GameObject GetCurrentPlayer()
    {
        return CurrentPlayer;
    }


    /**** 유니티 함수 ****/

    private void Awake()
    {
        photonManager = this;
        // 카메라 찾기
        //playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
        playerCamera = PlayerCamera.GetInstance();

        // 오브젝트 매니저 찾기
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        AllPlayers = new List<GameObject>();
    }

    private void Start()
    {

        // 플레이어 위치 씬 변경
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable { { "Scene", "InGame" } };
        PhotonNetwork.player.SetCustomProperties(ht);

        // 게임 시작

        condition = new Condition(CheckGameStart);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckGameStart");
        StartCoroutine(IEnumCoro);


    }


    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;
    }

    private bool isCanUseCursor = false;

    private void Update()
    {
        if (!isCanUseCursor)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            Debug.Log("옵저버모드 활성화");
            if (CurrentPlayer != null)
            {
                Vector3 position = CurrentPlayer.transform.position;

                PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerType", "Dead" } });

                AllPlayers.Remove(CurrentPlayer);

                PhotonNetwork.Destroy(CurrentPlayer);
                DramaticCameraObject = Instantiate(DramaticCameraPrefab, position, Quaternion.identity);

                Debug.Log("asdf");
            }   

        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (isCanUseCursor)
                isCanUseCursor = false;
            else
                isCanUseCursor = true;
        }
    }

    /**** 함수 ****/
    bool CheckEndTimer()
    {
        if (playTimerNumber <= 0)
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
        if (objectManager.InterObj.Count == 0)
        {
            return true;
        }
        return false;
    }

    // 게임이 끝났을 때 사용
    void ExecuteResult()
    {
        //ResultUI.SetActive(true);
    }


    // 플레이어 삭제
    void DeleteResult(int i)
    {


        Debug.Log("i : " + i);

        // 플레이어 삭제 처리
        Debug.Log("*************************12*" + CurrentPlayer + "********21**************");
        if (CurrentPlayer != null && photonView.isMine)
        {   
            
           Debug.Log("*************" + CurrentPlayer + "*****************");
          //  PhotonNetwork.Destroy(CurrentPlayer);
        }





        // 플레이어 UICanvas 끄기
        uIManager.hPPanelScript.SetHealthPoint(false);
        uIManager.mPPanelScript.SetManaPoint(false);
        uIManager.limitTimePanelScript.SetLimitTime(false);
        uIManager.SetAim(false);
        uIManager.BackgroundPanel.SetActive(false);

        // 혹시라도 도움말 UI가 켜져있으면 도움말 UI 제거
        uIManager.helpPanelScript.isCanUseHelperUI = false;
        uIManager.helpPanelScript.SetHelperUI(false);
        uIManager.helpPanelScript.SetHelpUI(false);

        // 별, 레스토랑 정보 끄기
        uIManager.starPanelScript.SetStarPanel(false);
        
        // 쥐 남은 수 끄기
        uIManager.mouseImagePanelScript.MouseImagePanel.SetActive(false);



        // 스코어 패널 끄기
        uIManager.scorePanelScript.ShowScorePanel(false);

        // 스코어 창 누르지 못하도록 변경.
        uIManager.scorePanelScript.IsUseScoreUI = false;



        // 플레이어 Result UI 설정
        uIManager.endStatePanelScript.SetEndState(true, (EndStatePanelScript.ResultType)i);


        // 플레이어 게임 종료 보여주기 
        Debug.Log("끝");
    }

    // 모든 플레이어가 한번씩 고양이를 했는지 여부 = 게임이 끝났는지.
    bool AllPlayCat()
    {
        bool isCheck = true;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["UseBoss"] == false)
            {
                isCheck = false;
            }
        }

        return isCheck;

    }



    /***** 조건용 함수들 *****/

    // 액션들 
    void NoAction()
    {
    }

    void DecreateTimeAction()
    {
        TimerValue -= Time.deltaTime;
    }

    // RPC Condition 들
    int NoRPCActonCondition()
    {
        return -1;
    }

    int MasterResultCheck()
    {
        int Type = -1;

        if (CheckAllBreak())
        {
            Debug.Log("!@");
            Type = 0;
        }
        else if (CheckMouseAllDead())
        {
            Debug.Log("!#");
            Type = 1;
        }
        else if (CheckEndTimer())
        {
            Debug.Log("!%");
            Type = 2;
        }

        if (Type == -1)
        {
            Debug.Log("에러발생");
            Type = 0;
        }

        return Type;
    }



    // 개인 사용자용 액션들
    
    void ExitGameRoom(params object[] obj)
    {
        PhotonNetwork.LeaveRoom();
    }




    // Condition 들

    // 로딩 끝났는지 파악
    bool CheckGameStart()
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
            }
        }

        return isInGame;
    }

     // 생성 끝났는지 파악
    bool CheckCreatePlayer()
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
            }
        }
        return isCreate;
    }

    // 게임 끝났는지 파악,
    bool CheckGameFinish()
    {
        if (CheckMouseAllDead() || CheckEndTimer() || CheckAllBreak())
        {
            if (CheckMouseAllDead())
                Debug.Log("1");
            if (CheckEndTimer())
                Debug.Log("2");
            if (CheckAllBreak())
                Debug.Log("3");
            return true;
        }
        else
        {
            return false;
        }
    }

    // 대기시간 기다릴 때 이 모두 흘렀는지 파악
    bool CheckTimeWait()
    {
        if (TimerValue <= 0)
            return true;
        else
            return false;
    }

    bool CheckGameEnd()
    {
        if (uIManager.scorePanelScript.IsUseNextButton == true)
            return true;
        else
            return false;
    }


    /**** 코루틴 ****/

        // 방장이 모든걸 처리함.
    // 조건/ 반복조건 / 액션조건판단 후 액션 으로 나눔
    IEnumerator CoroTrigger(Condition condition, ConditionLoop conditionLoop, RPCActionType rPCActionType, string RPCAction)
    {
        while (true)
        {

            bool AcceptCondition = condition();

            if (AcceptCondition)
            {
                if (PhotonNetwork.isMasterClient)
                {

                    // 액션 사용, 조건 판단해서 사용.
                    int type = rPCActionType();

                    if (type >= 0)
                        photonView.RPC(RPCAction, PhotonTargets.All, type);
                    else if (type == -1)
                        photonView.RPC(RPCAction, PhotonTargets.All);
                }
                yield break;
            }
            else
                conditionLoop();

            yield return null;
        }

    }

    IEnumerator CoroTriggerMine(Condition condition, ConditionLoop conditionLoop, ActionMine actionMine, params object[] obj)
    {
        while (true)
        {

            bool AcceptCondition = condition();

            if (AcceptCondition)
            {

                // 액션 사용, 조건 판단해서 사용.
                actionMine(obj);


                yield break;
            }
            else
                conditionLoop();

            yield return null;
        }
    }


    // 일반 코루틴

    IEnumerator Timer()
    {
        // 마스터 인 경우에만 실시.
        if (PhotonNetwork.isMasterClient)
        {
            uIManager.limitTimePanelScript.LimitTimeTextText.text = playTimerNumber.ToString();

            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                

                playTimerNumber -= 1;

                uIManager.limitTimePanelScript.LimitTimeTextText.text = playTimerNumber.ToString();
                if (playTimerNumber <= 0)
                {
                    yield break;
                }
            }
        }


        // 다른 클라이언트들은 갱신만 해주다가 마스터 값에 따라 종collisionObject료
        else
        {
            while (true) {

                uIManager.limitTimePanelScript.LimitTimeTextText.text = playTimerNumber.ToString();
                if (playTimerNumber <= 0)
                {
                    yield break;
                }
                yield return null;
            }
        }

    }







    /**** 서버전용) RPC 액션 함수 ****/

        // 고양이 생성 후 캐릭생성 판단하는 트리거 사용
    [PunRPC]
    void RPCActionCheckGameStart()
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
                    for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                    {
                        Debug.Log(i + " " + (bool)PhotonNetwork.playerList[i].CustomProperties["UseBoss"]);
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



        // 플레이어 생성 완료
        condition = new Condition(CheckCreatePlayer);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);

        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckCreatePlayer");
        StartCoroutine(IEnumCoro);
    }

    // UI 보여주고 게임 종료 조건 파악하는 트리거 사용
    [PunRPC]
    void RPCActionCheckCreatePlayer()
    {
        // 플레이어의 UI 매니저 받아옴
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        // 플레이어 정보를 추가합니다. 스코어 창에 쓰일꺼임.
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            uIManager.Players.Add(PhotonNetwork.playerList[i]);
        }

        // 도움말을 띄웁니다.
        uIManager.helpPanelScript.SetHelpUI(true);

        // 플레이어정보 소팅
        uIManager.PlayerSorting();



        // 1. UI들 보여주기
        uIManager.hPPanelScript.SetHealthPoint(true);
        uIManager.mPPanelScript.SetManaPoint(true);
        uIManager.limitTimePanelScript.SetLimitTime(true);
        uIManager.SetAim(true);
        uIManager.mouseImagePanelScript.MouseImagePanel.SetActive(true);
        uIManager.starPanelScript.StarPanel.SetActive(true);
        uIManager.BackgroundPanel.SetActive(true);


        // 2. UI에서 실시간으로 갱신해주도록 설정


        // 게임 종료 조건 시작
        condition = new Condition(CheckGameFinish);
        conditionLoop = new ConditionLoop(NoAction);
        rPCActionType = new RPCActionType(MasterResultCheck);
        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckGameFinish");
        StartCoroutine(IEnumCoro);


        // 타이머 시작
        IEnumCoro = Timer();
        StartCoroutine(IEnumCoro);

        // 노래 재생 시작
        SoundManager.GetInstance().PlayBackGround();

    }

    // UI삭제하고 일정 시간 대기하는 트리거 사용
    [PunRPC]
    void RPCActionCheckGameFinish(int Type)
    {

        // UI 삭제 후 시계모양 나타내줌
        DeleteResult(Type);

        // 대기 한 후 다음 코루틴 실행
        TimerValue = PreScoreUITimer;

        condition = new Condition(CheckTimeWait);
        conditionLoop = new ConditionLoop(DecreateTimeAction);
        rPCActionType = new RPCActionType(NoRPCActonCondition);
        IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCActionCheckFinishNext");
        StartCoroutine(IEnumCoro);
    }

    // 시간 지나면 Result 꺼버리고, 스코어창 보여주고. next활성화 시켜서 레디 확인하기.
    [PunRPC]
    void RPCActionCheckFinishNext()
    {
        Debug.Log("qwe");
        // 기존 Result창 꺼주기
        uIManager.endStatePanelScript.SetEndState(false, (EndStatePanelScript.ResultType)0);


        // 스코어창 보여주기
        uIManager.scorePanelScript.ShowScorePanel(true);


        // 게임 끝나면.
        if (AllPlayCat())
        {
            // 스코어창에서 추가로 보여주기
            uIManager.scorePanelScript.ShowScoreNextPanel(true);
            uIManager.scorePanelScript.ShowScoreEndBackGround(true);


            condition = new Condition(CheckGameEnd);
            conditionLoop = new ConditionLoop(NoAction);

            actionMine = new ActionMine(ExitGameRoom);

            isCanUseCursor = true;

            IEnumCoro = CoroTriggerMine(condition, conditionLoop, actionMine);
            StartCoroutine(IEnumCoro);
        }

        // 안끝났으면.
        else
        {
            TimerValue = 1.0f;
            condition = new Condition(CheckTimeWait);
            conditionLoop = new ConditionLoop(DecreateTimeAction);
            rPCActionType = new RPCActionType(NoRPCActonCondition);


            isCanUseCursor = false;

            IEnumCoro = CoroTrigger(condition, conditionLoop, rPCActionType, "RPCNextRound");
            StartCoroutine(IEnumCoro);
        }
    }


    // 다음 라운드로 이동
    [PunRPC]
    void RPCNextRound()
    {
        int data = (int)PhotonNetwork.player.CustomProperties["Round"];


        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Round", data+1 }  });
        PhotonNetwork.LoadLevel(2);
    }

    // 캐릭터 생성
    [PunRPC]
    void RPCCreatePlayer()
    {
        // 플레이어 생성
        string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

        // 고양이는 추가로 고양이가 될 수 없도록 해쉬값 생성
        if (PlayerType == "Cat")
        {
            CurrentPlayer = PhotonNetwork.Instantiate("Cat/CatBoss", MouseLocation.transform.position, MouseLocation.transform.rotation, 0);
            PhotonNetwork.player.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "UseBoss", true } });


            int NowCatScore = (int)PhotonNetwork.player.CustomProperties["CatScore"];

            ExitGames.Client.Photon.Hashtable CatScore = new ExitGames.Client.Photon.Hashtable { { "CatScore", NowCatScore + MaxCatScore } };
            PhotonNetwork.player.SetCustomProperties(CatScore);

        }

        else if (PlayerType == "Mouse")
            CurrentPlayer = PhotonNetwork.Instantiate("Mouse/MouseRunner", CatLocation.transform.position,CatLocation.transform.rotation, 0);


        // 생성했다는 의미로 오프셋 사용
        PhotonNetwork.player.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Offset", "CreateComplete" } });

    }


















    /**** 포톤 함수 ****/

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

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {

        // 리스트 내 플레이어 삭제
        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if (AllPlayers[i].GetPhotonView().ownerId == otherPlayer.ID)
            {
                Destroy(AllPlayers[i]);
                break;
            }
        }

        //사람이 넘어버리면 옵저버 재지정
        if (playerCamera.OverSeePlayer())
        {
            playerCamera.FindSeePlayer();
        }
        


    }



    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playTimerNumber);
        }

        else
        {
            playTimerNumber = (int)stream.ReceiveNext();
        }
    }

}




