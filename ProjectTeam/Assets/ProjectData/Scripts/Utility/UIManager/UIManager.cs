using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UIManager : Photon.PunBehaviour {

    public ScorePanelScript scorePanelScript { get; set; }





    [Header(" 스코어 이펙트 ")]
    public float GetScoreWaitTime = 0.5f;
    public float GetScoreMoveTime = 0.2f;


    // UI 플레이어 기준 수
    [Header("플레이어 UI 기준 몇명으로 할 것인가? ")]
    public int MaxUISlot = 6;
    

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




    // 1. 마우스 이미지 그릴지 말지 결정한다.
    private bool isDrawMouseImage = false;

    /**** 접근자 private ****/
    private PhotonManager photonManager;                // 포톤매니저

    public List<PhotonPlayer> Players{ get; set; }              // 플레이어를 담는 리스트 , PhotonManager에서 Add시킴

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

    public GameObject CatHelpUIImage { get; set; }
    public void InitCatHelpUIImage() { CatHelpUIImage = HelpUIPanel.transform.Find("CatHelpUIImage").gameObject; }
    public GameObject MouseHelpUIImage { get; set; }
    public void InitMouseHelpUIImage() { MouseHelpUIImage = HelpUIPanel.transform.Find("MouseHelpUIImage").gameObject; }


    public void SetHelperUI(bool isActive)
    {
        HelperUIImage.SetActive(isActive);
        HelperUIPanel.SetActive(isActive);
    }

    public void SetHelpUI(bool isActive)
    {
        HelpUIPanel.SetActive(isActive);

        CatHelpUIImage.SetActive(false);
        MouseHelpUIImage.SetActive(false);

        // MouseHelpUIImage.SetActive(false);
        if (isActive)
        {
            string PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

            if (PlayerType == "Cat")
            {
                CatHelpUIImage.SetActive(true);

            }
            else if (PlayerType == "Mouse")
            {

                MouseHelpUIImage.SetActive(true);

            }

        }

        else
        {

            
        }

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




    





    /********************** InGameCanvas ******************/
    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas() { InGameCanvas = GameObject.Find("InGameCanvas"); }

    public GameObject GetScorePanel { get; set; }
    public void InitGetScorePanel() { GetScorePanel = InGameCanvas.transform.Find("GetScorePanel").gameObject; }



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
            star = 0;
        }


        OnStarGrade(star);

        // 레스토랑 이미지 설정


        // 쥐 이미지 갱신


    }

    public void CreateScoreImage(int Score)
    {
        // 오브젝트 풀.

        //1 . 풀링에서 빼옵니다.
        GameObject getScoreText = PoolingManager.GetInstance().PopObject("GetScoreText");

        // 풀링의 텍스트 이름 수정
        getScoreText.GetComponent<Text>().text = "+" + Score.ToString();

        //2. 부모 설정
        getScoreText.transform.SetParent(GetScorePanel.transform);

        //3. 초기 위치 설정.
        Vector3 v3 = new Vector3 { x = Screen.width * 0.7f, y = Screen.height * 0.55f, z = 90.0f };

        getScoreText.transform.localScale = Vector3.one;
        getScoreText.transform.position = v3;


        // 4. 이미지를 움직인다. 
        EnumCoro = MoveScoreImage(getScoreText);

        StartCoroutine(EnumCoro);
    }




    /**** 유니티 함수 ****/
    private void Awake()
    {
        uIManager = this;

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
        InitCatHelpUIImage();
        InitMouseHelpUIImage();


        InitAimPanel();
        InitAimImage();


        InitStarPanel();
        InitStarImage();

        InitMouseImagePanel();
        InitMouseImage();
        InitMouseOffImage();




        InitResultUI();


        InitEndStatePanel();
        InitAllBreakImage();
        InitAllKillImage();
        InitTimeOverImage();

        InitInGameCanvas();

        InitGetScorePanel();

        // 스코어 패널 관련 내용 스크립트로 따로 처리
        scorePanelScript = new ScorePanelScript();
        scorePanelScript.InitData(MaxUISlot);


    }


    private void Update()
    {
        // 스코어 패널 보여줌
        ShowScore();

        // 스코어 갱신 
        UpdateScore();

        // 도움말 사용 가능할 때
        OnOffHelpUI();


        // 쥐, 레스토랑 모양 갱신
        float ObjectPersent = ((float)objectManager.InterObj.Count / (float)objectManager.MaxInterObj) * 100;

        CheckRestUI(ObjectPersent);
        SetNowMouseImage();

    }


    void ShowScore()
    {
        // 스코어 사용 가능할 때
        if (scorePanelScript.IsUseScoreUI == true)
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {

                if (scorePanelScript.ScorePanel.GetActive() == false)
                {
                    scorePanelScript.ShowScorePanel(true);
                }

                else
                {
                    scorePanelScript.ShowScorePanel(false);
                }
            }

        }

    }

    void UpdateScore()
    {
        if (scorePanelScript.ScorePanel.GetActive() == true)
        {
            scorePanelScript.UpdateScores();
        }
    }

    void OnOffHelpUI()
    {
        // 도움말 사용 가능할 때
        if (isCanUseHelperUI)
        {
            if (Input.GetKeyDown(KeyCode.F1) ||
                Input.GetKeyDown(KeyCode.Escape))
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
                    GameObject hit = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.STAR_HIT);
                    //hit.transform.position = camera.worldto

                    Camera c = Camera.main;
                    Vector3 v3 = c.WorldToScreenPoint(transform.position);

                    hit.transform.position = v3;

                    PoolingManager.GetInstance().PushObject(go);
                    yield break;
                }

                else
                    yield return null;
            }




        }

    }


    /**** UI 이벤트  ****/


    // UI 이벤트

    public void ClickScoreNextButton()
    {
        scorePanelScript.IsUseNextButton = true;
    }

}
