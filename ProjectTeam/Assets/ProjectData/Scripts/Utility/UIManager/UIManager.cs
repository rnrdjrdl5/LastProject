using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UIManager : Photon.PunBehaviour {

    [Header(" 스코어 이펙트 ")]
    public float GetScoreWaitTime = 0.5f;
    public float GetScoreMoveTime = 0.2f;

    [Header("최대인원")]
    public int MaxPlayer = 6;
    
    IEnumerator EnumCoro;





    static private UIManager uIManager;

    static public UIManager GetInstance()
    {
        return uIManager;
    }


    /**** Type ****/

    public enum ResultType { BREAK,KILL,TIMEOVER }

    private int OneStarCondition;
    private int TwoStarCondition;
    private int ThreeStarCondition;
    private int ForeStarCondition;
    private int FiveStarCondition;

    private bool isUseNextButton = false;



    // 1. 마우스 이미지 그릴지 말지 결정한다.
    private bool isDrawMouseImage = false;

    /**** 접근자 private ****/
    private PhotonManager photonManager;                // 포톤매니저

    public List<PhotonPlayer> Players{ get; set; }              // 플레이어를 담는 리스트 , PhotonManager에서 Add시킴

    public bool IsScoreReCheck { get; set; }             // 플레이어 정보 갱신시킬지 말지 결정함
    public bool IsUseScoreUI { get; set; }

    public ObjectManager objectManager;









    /************************************ UICanvas ***********************************************/





    /**** UICanvas ****/

    public GameObject UICanvas { set; get; }
    public void InitUICanvas() { UICanvas = GameObject.Find("UICanvas");
    }

    /**** BackgroundPanel ****/
    public GameObject BackgroundPanel { get; set; }
    public void InitBackgroundPanel()
    {
        BackgroundPanel = UICanvas.transform.Find("BackgroundPanel").gameObject;
    }


    /**** LimitTime ****/

    public GameObject LimitTimePanel { set; get; }
    public void InitLimitTimePanel() { LimitTimePanel = UICanvas.transform.Find("LimitTimePanel").gameObject; }

    public GameObject LimitTimeText { set; get; }
    public void InitLimitTimeText() { LimitTimeText = LimitTimePanel.transform.Find("LimitTimeText").gameObject; }

    public Text LimitTimeTextText { set; get; }
    public void InitLimitTimeTextText() { LimitTimeTextText = LimitTimeText.GetComponent<Text>(); }

    public void SetLimitTime(bool isActive)
    {
        LimitTimePanel.SetActive(isActive);
        LimitTimeText.SetActive(isActive);
    }



    /**** HP ****/

    public GameObject HPPanel { set; get; }
    public void InitHPPanel() { HPPanel = UICanvas.transform.Find("HPPanel").gameObject; }

    public GameObject NowHpImage { set; get; }
    public void InitNowHPImage() {
        NowHpImage = HPPanel.transform.Find("NowHpImage").gameObject; }

    public void SetHealthPoint(bool isActive)
    {
        HPPanel.SetActive(isActive);
        NowHpImage.SetActive(isActive);
    }


    /**** MP ****/

    public GameObject MPPanel { set; get; }
    public void InitMPPanel() { MPPanel = UICanvas.transform.Find("MPPanel").gameObject; }

    public GameObject NowMpImage { set; get; }
    public void InitNowMPImage() { NowMpImage = MPPanel.transform.Find("NowMpImage").gameObject; }

    public void SetManaPoint(bool isActive)
    {
        MPPanel.SetActive(isActive);
        NowMpImage.SetActive(isActive);
    }


    /**** HelperUI , HelpUI  , Helper : Icon ****/


    public bool isCanUseHelperUI = true;

    public GameObject HelperUIPanel { set; get; }
    public void InitHelperUIPanel() { HelperUIPanel = UICanvas.transform.Find("HelperUIPanel").gameObject; }
    public GameObject HelpUIPanel { set; get; }
    public void InitHelpUIPanel() { HelpUIPanel = UICanvas.transform.Find("HelpUIPanel").gameObject; }



    public GameObject HelperUIImage { set; get; }
    public void InitHelperUIImage() { HelperUIImage = HelperUIPanel.transform.Find("HelperUIImage").gameObject; }
    public GameObject HelpUIImage { set; get; }
    public void InitHelpUIImage() { HelpUIImage = HelpUIPanel.transform.Find("HelpUIImage").gameObject; }

    public void SetHelpUI(bool isActive)
    {
        HelpUIImage.SetActive(isActive);
        HelpUIPanel.SetActive(isActive);
    }
    public void SetHelperUI(bool isActive)
    {
        HelperUIImage.SetActive(isActive);
        HelperUIPanel.SetActive(isActive);
    }


    /**** AimPanel ****/

    public GameObject AimPanel { set; get; }
    public void InitAimPanel() { AimPanel = UICanvas.transform.Find("AimPanel").gameObject; }

    public GameObject AimImage { set; get; }
    public void InitAimImage() { AimImage = AimPanel.transform.Find("AimImage").gameObject; }    

    public void SetAim(bool isActive)
    {
        AimPanel.SetActive(isActive);
        AimImage.SetActive(isActive);
    }


    /***** StartPanel *****/

    public GameObject StarPanel;
    public void InitStarPanel() { StarPanel = UICanvas.transform.Find("StarPanel").gameObject; }

    public GameObject[] StarImage;
    public void InitStarImage()
    {
        StarImage = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {

            StarImage[i] = StarPanel.transform.Find("StarImage" + (i + 1).ToString()).gameObject;
        }
    }


    // 특정 번호의 별을 설정
    public void SetStarNumber(bool isActive, int type)
    {
        StarImage[type - 1].SetActive(isActive);
    }

    //별을 순차별로 정함.
    public void OnStarGrade(int Grade)
    {
        for (int i = 0; i < StarImage.Length; i++)
        {
            if (i < Grade)
                StarImage[i].SetActive(true);
        
            else
                StarImage[i].SetActive(false);
        }

      //  SetStarPanel(true);
    }

    // 모든 별들에게 공통적으로 실시
    public void SetStar(bool isActive)
    {
        for (int i = 0; i < StarImage.Length; i++)
        {
            StarImage[i].SetActive(isActive);
        }

        StarPanel.SetActive(isActive);
    }

    // 패널 설정
    public void SetStarPanel(bool isActive)
    {
        StarPanel.SetActive(isActive);
    }



    /**** MouseImagePanel ****/
    public GameObject MouseImagePanel;
    public void InitMouseImagePanel() { MouseImagePanel = UICanvas.transform.Find("MouseImagePanel").gameObject; }

    public GameObject[] MouseImage;
    public void InitMouseImage()
    {
        MouseImage = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            MouseImage[i] = MouseImagePanel.transform.Find("MouseImage" + (i + 1).ToString()).gameObject;
        }
    }

    public GameObject[] MouseOffImage;
    public void InitMouseOffImage()
    {
        MouseOffImage = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            MouseOffImage[i] = MouseImagePanel.transform.Find("MouseOffImage" + (i + 1).ToString()).gameObject;
        }
    }

    public void SetNowMouseImage()
    {
        // 0. 현재 접속유저 판단.

        int MouseAmount = PhotonNetwork.playerList.Length - 1;

        for (int i = 0; i < 5; i++)
        {
            

            if (i < MouseAmount)
            {
                MouseOffImage[i].SetActive(false);
                MouseImage[i].SetActive(false);
            }
        }

        // 갯수는 모두 돌자.
        int NowLiveMouse = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Mouse")
            {

                // 1. 쥐면 추가한다.
                NowLiveMouse++;
            }

        }

        // 1 최대 갯수 설정

        for (int i = 0; i < MouseAmount; i++)
        {
            if (i < NowLiveMouse)
            {
                MouseImage[i].SetActive(true);
            }

            else
            {
                MouseImage[i].SetActive(false);
                MouseOffImage[i].SetActive(true);
            }

        }


    }

    




    /************************************ ResultUI ***********************************************/


    /**** ResultUI ****/

    public GameObject ResultUI { set; get; }
    public void InitResultUI() { ResultUI = GameObject.Find("ResultUI"); }


    /**** TimeWatchPanel ****/

    public GameObject TimeWatchPanel { set; get; }
    public void InitTimeWatchPanel() { TimeWatchPanel = ResultUI.transform.Find("TimeWatchPanel").gameObject; }

    public void SetTimeWatch(bool isActive)
    {
        TimeWatchPanel.SetActive(isActive);
    }

    /**** EndStatePanel ****/

    public GameObject EndStatePanel { set; get; }
    public void InitEndStatePanel() {
        EndStatePanel = ResultUI.transform.Find("EndStatePanel").gameObject; }


    public GameObject AllBreakImage { set; get; }
    public void InitAllBreakImage() { AllBreakImage = EndStatePanel.transform.Find("AllBreakImage").gameObject; }
    public GameObject AllKillImage { set; get; }
    public void InitAllKillImage() { AllKillImage = EndStatePanel.transform.Find("AllKillImage").gameObject; }
    public GameObject TimeOverImage { set; get; }
    public void InitTimeOverImage() { TimeOverImage = EndStatePanel.transform.Find("TimeOverImage").gameObject; }

    public void SetEndState(bool isActive, ResultType rT)
    {
        switch (rT)
        {

            case ResultType.BREAK:
                AllBreakImage.SetActive(isActive);
                break;
            case ResultType.KILL:
                AllKillImage.SetActive(isActive);
                break;
            case ResultType.TIMEOVER:
                TimeOverImage.SetActive(isActive);
                break;
        }
        Debug.Log((ResultType)rT);

        EndStatePanel.SetActive(isActive);
    }

        



    /**** ScroePanel ****/

    public GameObject ScorePanel { set; get; }
    public void InitScorePanel() { ScorePanel = ResultUI.transform.Find("ScorePanel").gameObject; }

    public void SetScorePanel(bool isActive)
    {
        ScorePanel.SetActive(isActive);
    }


    /**** Mouse Score ****/
    public GameObject MouseScoreBGImage { set; get; }
    public void InitMouseScoreBGImage() { MouseScoreBGImage = ScorePanel.transform.Find("MouseScoreBGImage").gameObject; }

    public Text[] MousePlayerName { set; get; }

    public Text[] MousePlayerScore { set; get; }

    // MousePlayer 이름 정보 재설정
    public void InitMousePlayerStats()
    {
        MousePlayerName = new Text[MaxPlayer];
        MousePlayerScore = new Text[MaxPlayer];

        for (int i = 0; i < MaxPlayer; i++)
        {
            MousePlayerName[i] = MouseScoreBGImage.transform.Find("MousePlayer" + (i + 1).ToString() + "BGImage/PlayerName").GetComponent<Text>();

            MousePlayerScore[i] = MouseScoreBGImage.transform.Find("MousePlayer" + (i + 1).ToString() + "BGImage/PlayerScore").GetComponent<Text>();
        }
    }

    /**** Cat Score ****/
    public GameObject CatScoreBGImage { set; get; }
    public void InitCatScoreBGImage() { CatScoreBGImage = ScorePanel.transform.Find("CatScoreBGImage").gameObject; }


    public Text[] CatPlayerName { set; get; }

    public Text[] CatPlayerScore { set; get; }

    // CatPlayer 이름 정보 재설정
    public void InitCatPlayerStats()
    {
        CatPlayerName = new Text[MaxPlayer];
        CatPlayerScore = new Text[MaxPlayer];

        for (int i = 0; i < MaxPlayer; i++)
        {

            CatPlayerName[i] = CatScoreBGImage.transform.Find("CatPlayer" + (i + 1).ToString() + "BGImage/PlayerName").GetComponent<Text>();

            CatPlayerScore[i] = CatScoreBGImage.transform.Find("CatPlayer" + (i + 1).ToString() + "BGImage/PlayerScore").GetComponent<Text>();
        }
    }



    /**** Total Score ****/
    public GameObject TotalScoreBGImage { set; get; }
    public void InitTotalScoreBGImage() { TotalScoreBGImage = ScorePanel.transform.Find("TotalScoreBGImage").gameObject; }

    public Text[] TotalPlayerName { set; get; }

    public Text[] TotalPlayerScore { set; get; }

    // TotalPlayer 이름 재설정
    public void InitTotalPlayerStats()
    {
        TotalPlayerName = new Text[MaxPlayer];
        TotalPlayerScore = new Text[MaxPlayer];

        for (int i = 0; i < MaxPlayer; i++)
        {
            TotalPlayerName[i] = TotalScoreBGImage.transform.Find("TotalPlayer" + (i+1).ToString() + "BGImage/PlayerName").GetComponent<Text>();
            
            TotalPlayerScore[i] = TotalScoreBGImage.transform.Find("TotalPlayer" + (i + 1).ToString() + "BGImage/PlayerScore").GetComponent<Text>();
        }
    }


    /**** SelectPlayerPanel ****/
    public GameObject SelectPlayerPanel { get; set; }
    public void InitSelectPlayerPanel() { SelectPlayerPanel = ScorePanel.transform.Find("SelectPlayerPanel").gameObject; }

    public GameObject[] SelectPlayerImage { get; set; }

    public void InitSelectPlayerImage()
    {
        SelectPlayerImage = new GameObject[MaxPlayer];

        for (int i = 0; i < SelectPlayerImage.Length; i++)
        {
            SelectPlayerImage[i] = SelectPlayerPanel.transform.Find("SelectPlayerImage" + (i + 1).ToString()).gameObject;
        }
    }

    /**** GameEndPanel ****/
    public GameObject GameEndPanel { get; set; }
    public void InitGameEndPanel() { GameEndPanel = ScorePanel.transform.Find("GameEndPanel").gameObject; }
    public bool IsGameEnd { get; set; }







    /********************** InGameCanvas ******************/
    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas() { InGameCanvas = GameObject.Find("InGameCanvas"); }

    public GameObject GetScorePanel { get; set; }
    public void InitGetScorePanel() { GetScorePanel = InGameCanvas.transform.Find("GetScorePanel").gameObject; }



    /***** 일반 함수 *****/
    void SetScoreMenu()
    {
        
        // Sort 정렬
        PlayerSorting();

        // 패널 수만큼 루프
        for (int i = 0; i < MaxPlayer; i++)
        {

            // 인원 수만큼 정보 설정
            if (i < Players.Count)
            {
                int CatScore = -1;
                if (Players[i].CustomProperties["CatScore"] != null)
                     CatScore= (int)Players[i].CustomProperties["CatScore"];


                int MouseScore = -1;
                if (Players[i].CustomProperties["MouseScore"] != null)
                    MouseScore = (int)Players[i].CustomProperties["MouseScore"];

                
                // 점수 설정
                MousePlayerScore[i].text = MouseScore.ToString();
                CatPlayerScore[i].text = CatScore.ToString();
                TotalPlayerScore[i].text = (MouseScore + CatScore).ToString();

                // 이름 설정
                CatPlayerName[i].text = Players[i].NickName;
                MousePlayerName[i].text = Players[i].NickName;
                TotalPlayerName[i].text = Players[i].NickName;

                // 본인 클라이언트면 불빛 들어오도록 설정
                if (Players[i].NickName == PhotonNetwork.player.NickName)
                {
                    SelectPlayerImage[i].SetActive(true);
                }
                else
                    SelectPlayerImage[i].SetActive(false);
            }
            
            // 인원 아닌 곳 공백 처리
            else
            {
                MousePlayerScore[i].text = "";
                CatPlayerScore[i].text = "";
                TotalPlayerScore[i].text = "";

                CatPlayerName[i].text = "";
                MousePlayerName[i].text = "";
                TotalPlayerName[i].text = "";

                SelectPlayerImage[i].SetActive(false);
            }
        }

    }

    // 플레이어 리스트 Sort
    public void PlayerSorting()
    {
        Players.Sort(
            (PhotonPlayer One, PhotonPlayer Two) =>
            {
                if (One.ID > Two.ID)
                    return 1;
                else if (One.ID < Two.ID)
                    return -1;
                return 0;

            }
            );
    }

    public void OnScorePanel()
    {
        SetScorePanel(true);
        IsScoreReCheck = true;
    }

    public void OffScorePanel()
    {
        SetScorePanel(false);
        IsScoreReCheck = false;
    }

    public void CheckRestUI(float Type)
    {
        // 별 설정
        int star = 0;
        if (Type >= FiveStarCondition)
        {
            star = 5;
        }
        else if (Type >= ForeStarCondition && Type < FiveStarCondition)
        {
            star = 4;
        }
        else if (Type >= ThreeStarCondition && Type < ForeStarCondition)
        {
            star = 3;
        }
        else if (Type >= TwoStarCondition && Type < ThreeStarCondition)
        {
            star = 2;
        }
        else if (Type >= OneStarCondition && Type < TwoStarCondition)
        {
            star = 1;
        }
        else
        {
            Debug.Log("조건충족안됨");
            star = 0;
        }


        OnStarGrade(star);

        // 레스토랑 이미지 설정


        // 쥐 이미지 갱신


    }

    public void CreateScoreImage()
    {
        // 오브젝트 풀.

        //1 . 풀링에서 빼옵니다.
        GameObject getScoreImageObject = PoolingManager.GetInstance().PopObject("GetScoreImage");

        //2. 부모 설정
        getScoreImageObject.transform.SetParent(GetScorePanel.transform);

        //3. 초기 위치 설정.
        Vector3 v3 = new Vector3 { x = Screen.width * 0.7f, y = Screen.height * 0.55f, z = 90.0f };

        getScoreImageObject.transform.localScale = Vector3.one;
        getScoreImageObject.transform.position = v3;


        // 4. 이미지를 움직인다. 
        EnumCoro = MoveScoreImage(getScoreImageObject);

        StartCoroutine(EnumCoro);
    }




    /**** 유니티 함수 ****/
    private void Awake()
    {
        uIManager = this;

        // 플레이어 갱신 X 설정
        IsScoreReCheck = false;

        // 스코어 UI 사용 가능
        IsUseScoreUI = true;

        // 플레이어 리스트 초기화
        Players = new List<PhotonPlayer>();

        // 오브젝트 매니저 초기화
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();


        if (photonManager != null)
        {

            OneStarCondition = photonManager.OneStarCondition;    
            TwoStarCondition = photonManager.TwoStarCondition; 
            ThreeStarCondition = photonManager.ThreeStarCondition; 
            ForeStarCondition = photonManager.ForeStarCondition;  
            FiveStarCondition = photonManager.FiveStarCondition;  

        }



    // UI 초기화들
    InitUICanvas();

        InitBackgroundPanel();

        InitLimitTimePanel();
        InitLimitTimeText();
        InitLimitTimeTextText();

        InitHPPanel();
        InitNowHPImage();

        InitMPPanel();
        InitNowMPImage();

        InitHelperUIPanel();
        InitHelperUIImage();

        InitHelpUIPanel();
        InitHelpUIImage();

        InitAimPanel();
        InitAimImage();


        InitStarPanel();
        InitStarImage();

        InitMouseImagePanel();
        InitMouseImage();
        InitMouseOffImage();




        InitResultUI();

        InitTimeWatchPanel();

        InitEndStatePanel();
        InitAllBreakImage();
        InitAllKillImage();
        InitTimeOverImage();

        InitScorePanel();

        InitMouseScoreBGImage();
        InitMousePlayerStats();

        InitCatScoreBGImage();
        InitCatPlayerStats();

        InitTotalScoreBGImage();
        InitTotalPlayerStats();

        InitSelectPlayerPanel();
        InitSelectPlayerImage();



        InitInGameCanvas();

        InitGetScorePanel();

        InitGameEndPanel();
        IsGameEnd = false;
    }


    private void Update()
    {
        // 스코어 사용 가능할 때
        if (IsUseScoreUI)
        {
            // Tap 누를 때마다 재설정
            if (Input.GetKeyDown(KeyCode.Tab))
            {

                // 비활성화 일 경우
                if (ScorePanel.GetActive() == false)
                {
                    OnScorePanel();
                }

                // 활성화 일 경우
                else if (ScorePanel.GetActive() == true)
                {
                    OffScorePanel();
                }
                else
                    Debug.Log("에러");
            }

        }

        // 스코어 갱신중일때
        if (IsScoreReCheck)
        {
            SetScoreMenu();
        }

        // 도움말 사용 가능할 때
        if (isCanUseHelperUI)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (HelperUIPanel.GetActive() == true)
                {
                    SetHelperUI(false);
                    SetHelpUI(true);
                }
                else
                {
                    SetHelperUI(true);
                    SetHelpUI(false);
                }
            }
        }


        // 쥐, 레스토랑 모양 갱신
        float ObjectPersent = ((float)objectManager.InterObj.Count / (float)objectManager.MaxInterObj) * 100;

        CheckRestUI(ObjectPersent);
        SetNowMouseImage();


        

    }
        
    /**** UI 이벤트  ****/
    public void GameEndButtonClick()
    {
        if (IsGameEnd)
            IsGameEnd = false;
        else
            IsGameEnd = true;

        Debug.Log("end : " + IsGameEnd);


    }






    enum EnumMoveScore { WAIT, MOVE };
    IEnumerator MoveScoreImage(GameObject go)
    {
        // 1. 이미지 원래 위치 받아온다.
        Vector3 OriginalPosition = go.transform.position;

        // 2. 이미지 마지막위치 선정
        Vector3 FinalPosition = new Vector3 { x = Screen.width * 0.9f, y = Screen.height * 0.9f, z = 0.0f };

        // 3. Lerp에 쓰일 초기 값을 정한다.
        float NowTime = 0.0f;


        EnumMoveScore MoveScoreType = EnumMoveScore.WAIT;
        // 2가지 단계로 나눈다.
        // 1. 머무르는 시간
        // 2. 날라가는 시간

    

        while (true)
        {

            if (MoveScoreType == EnumMoveScore.WAIT)
            {
                if (NowTime >= GetScoreWaitTime)
                {
                    MoveScoreType = EnumMoveScore.MOVE;
                    NowTime = 0.0f;

                }
                else
                {
                    NowTime += Time.deltaTime;
                    yield return null;
                }
            }
            

            if (MoveScoreType == EnumMoveScore.MOVE)
            {

                // 1. 이미지의 다음값을 선정
                NowTime += 1 / GetScoreMoveTime * Time.deltaTime;

                // 2. 다음 위치로 이동
                go.transform.position = Vector3.Lerp(OriginalPosition, FinalPosition, NowTime);

                // 3. 위치가 1 이상이면 오브젝트 풀에 다시 넣고 끝낸다.
                if (NowTime >= 1.0f)
                {
                    PoolingManager.GetInstance().PushObject(go);
                    yield break;
                }

                else
                    yield return null;
            }




        }

    }

}
