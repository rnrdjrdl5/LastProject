using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UIManager : Photon.PunBehaviour {

    /**** Type ****/

    public enum ResultType { BREAK,KILL,TIMEOVER }

    private int OneStarCondition;
    private int TwoStarCondition;
    private int ThreeStarCondition;
    private int ForeStarCondition;
    private int FiveStarCondition;

    // 레스토랑 이미지 최소 설정값
    private int OneRestState;
    private int TwoRestState;
    private int ThreeRestState;

    /**** 접근자 private ****/
    private PhotonManager photonManager;                // 포톤매니저

    public List<PhotonPlayer> Players{ get; set; }              // 플레이어를 담는 리스트 , PhotonManager에서 Add시킴

    private bool IsScoreReCheck;              // 플레이어 정보 갱신시킬지 말지 결정함
    public bool IsUseScoreUI { get; set; }

    public ObjectManager objectManager;

    






    /************************************ UICanvas ***********************************************/





    /**** UICanvas ****/

    public GameObject UICanvas { set; get; }
    public void InitUICanvas() { UICanvas = GameObject.Find("UICanvas");
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


    /**** StartRun ****/

    public GameObject StartRunPanel { set; get; }
    public void InitStartRunPanel() { StartRunPanel = UICanvas.transform.Find("StartRunPanel").gameObject; }

    public GameObject StartRunText { set; get; }
    public void InitStartRunText() { StartRunText = StartRunPanel.transform.Find("StartRunText").gameObject; }

    public Text StartRunTextText { set; get; }
    public void InitStartRunTextText() { StartRunTextText = StartRunText.GetComponent<Text>(); }

    public void SetStartRun(bool isActive)
    {
        StartRunPanel.SetActive(isActive);
        StartRunText.SetActive(isActive);
    }

    /**** HelperUI , HelpUI  , Helper : Icon ****/

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

    /***** RestStatePanel *****/

    public GameObject RestStatePanel;
    public void InitRestStatePanel() { RestStatePanel = UICanvas.transform.Find("RestStatePanel").gameObject; }

    public GameObject[] RestStates;
    public void InitRestStates()
    {
        RestStates = new GameObject[3];
        Debug.Log(RestStatePanel);
        for (int i = 0; i < 3; i++)
        {
            RestStates[i] = RestStatePanel.transform.Find("RestState" + (i + 1).ToString()).gameObject;
        }
    }

    public void SetRestState(bool isActive, int type)
    {
        for (int i = 0; i < RestStates.Length; i++)
        {

            RestStates[i].SetActive(false);
        }

        RestStates[type - 1].SetActive(isActive);
        RestStatePanel.SetActive(isActive);
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
        for (int i = 0; i < 5; i++)
        {
            MouseImage[i] = MouseImagePanel.transform.Find("MouseImage" + (i + 1).ToString()).gameObject;
        }
    }

    public GameObject[] MouseOffImage;
    public void InitMouseOffImage()
    {
        for (int i = 0; i < 5; i++)
        {
            MouseOffImage[i] = MouseImagePanel.transform.Find("MouseOffImage" + (i + 1).ToString()).gameObject;
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

    public GameObject MouseScoreBGImage { set; get; }
    public void InitMouseScoreBGImage() { MouseScoreBGImage = ScorePanel.transform.Find("MouseScoreBGImage").gameObject; }

    public Text[] MousePlayerName { set; get; }

    public Text[] MousePlayerScore { set; get; }

    // MousePlayer 이름 정보 재설정
    public void InitMousePlayerStats()
    {
        MousePlayerName = new Text[6];
        MousePlayerScore = new Text[6];

        for (int i = 0; i < 6; i++)
        {
            MousePlayerName[i] = MouseScoreBGImage.transform.Find("MousePlayer" + (i + 1).ToString() + "BGImage/PlayerName").GetComponent<Text>();

            MousePlayerScore[i] = MouseScoreBGImage.transform.Find("MousePlayer" + (i + 1).ToString() + "BGImage/PlayerScore").GetComponent<Text>();
        }
    }






    public GameObject CatScoreBGImage { set; get; }
    public void InitCatScoreBGImage() { CatScoreBGImage = ScorePanel.transform.Find("CatScoreBGImage").gameObject; }


    public Text[] CatPlayerName { set; get; }

    public Text[] CatPlayerScore { set; get; }

    // CatPlayer 이름 정보 재설정
    public void InitCatPlayerStats()
    {
        CatPlayerName = new Text[6];
        CatPlayerScore = new Text[6];

        for (int i = 0; i < 6; i++)
        {

            CatPlayerName[i] = CatScoreBGImage.transform.Find("CatPlayer" + (i + 1).ToString() + "BGImage/PlayerName").GetComponent<Text>();

            CatPlayerScore[i] = CatScoreBGImage.transform.Find("CatPlayer" + (i + 1).ToString() + "BGImage/PlayerScore").GetComponent<Text>();
        }
    }




    public GameObject TotalScoreBGImage { set; get; }
    public void InitTotalScoreBGImage() { TotalScoreBGImage = ScorePanel.transform.Find("TotalScoreBGImage").gameObject; }

    public Text[] TotalPlayerName { set; get; }

    public Text[] TotalPlayerScore { set; get; }

    // TotalPlayer 이름 재설정
    public void InitTotalPlayerStats()
    {
        TotalPlayerName = new Text[6];
        TotalPlayerScore = new Text[6];

        for (int i = 0; i < 6; i++)
        {
            TotalPlayerName[i] = TotalScoreBGImage.transform.Find("TotalPlayer" + (i+1).ToString() + "BGImage/PlayerName").GetComponent<Text>();
            
            TotalPlayerScore[i] = TotalScoreBGImage.transform.Find("TotalPlayer" + (i + 1).ToString() + "BGImage/PlayerScore").GetComponent<Text>();
        }
    }



    /***** 일반 함수 *****/
    void SetScoreMenu()
    {
        
        // Sort 정렬
        PlayerSorting();

        // 패널 수만큼 루프
        for (int i = 0; i < 6; i++)
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





    /**** 유니티 함수 ****/
    private void Awake()
    {
        // 플레이어 갱신 X 설정
        IsScoreReCheck = false;

        // 스코어 UI 사용 가능
        IsUseScoreUI = true;

        // 플레이어 리스트 초기화
        Players = new List<PhotonPlayer>();

        // 오브젝트 매니저 초기화
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();



        OneStarCondition = photonManager.OneStarCondition;
        TwoStarCondition = photonManager.TwoStarCondition;
        ThreeStarCondition = photonManager.ThreeStarCondition;
        ForeStarCondition = photonManager.ForeStarCondition;
        FiveStarCondition = photonManager.FiveStarCondition;

        OneRestState = photonManager.OneRestState;
        TwoRestState = photonManager.TwoRestState;
        ThreeRestState = photonManager.ThreeRestState;



    // UI 초기화들
    InitUICanvas();

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

        InitStartRunPanel();
        InitStartRunText();
        InitStartRunTextText();

        InitAimPanel();
        InitAimImage();

        InitRestStatePanel();
        InitRestStates();

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
    }


    private void Update()
    {

        // 본인 인 경우에만
        if (photonView.isMine)
        {

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


                // 갱신중일때
                if (IsScoreReCheck)
                {
                    SetScoreMenu();
                }




                

            }


            float ObjectPersent = objectManager.InterObj.Count / objectManager.MaxInterObj * 100;
            Debug.Log(ObjectPersent);

            CheckRestUI(ObjectPersent);

        }

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
        if (Type >= photonManager.FiveStarCondition)
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
            star = 0;

            OnStarGrade(star);

        // 레스토랑 이미지 설정

        if (Type >= ThreeRestState)
        {
            star = 3;
        }
        else if (Type >= TwoRestState && Type < ThreeRestState)
        {
            star = 2;
        }
        else if (Type >= OneRestState && Type < TwoRestState)
        {
            star = 1;
        }
        else
            star = 0;


        SetRestState(true, star);

    }

}
