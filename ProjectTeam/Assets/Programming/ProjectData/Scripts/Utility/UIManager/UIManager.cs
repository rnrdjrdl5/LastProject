using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UIManager : Photon.PunBehaviour {

    public ScorePanelScript scorePanelScript { get; set; }
    public TimeBarPanelScript timerBarPanelScript { get; set; }
    public HelpPanelScript helpPanelScript { get; set; }
    public StarPanelScript starPanelScript { get; set; }
    public MouseImagePanelScript mouseImagePanelScript { get; set; }
    public GetScoreImageScript getScoreImageScript { get; set; }
    public EndStatePanelScript endStatePanelScript { get; set; }
    public HPPanelScript hPPanelScript { get; set; }
    public LimitTimePanelScript limitTimePanelScript { get; set; }
    public MPPanelScript mPPanelScript { get; set; }

    public delegate void DeleUpdate();
    public event DeleUpdate UpdateEvent;


    // UI 플레이어 기준 수
    [Header("플레이어 UI 기준 몇명으로 할 것인가? ")]
    public int MaxUISlot = 6;
    

    IEnumerator EnumCoro;





    static private UIManager uIManager;

    static public UIManager GetInstance()
    {
        return uIManager;
    }

    /**** 접근자 private ****/
    private PhotonManager photonManager;                // 포톤매니저

    public List<PhotonPlayer> Players{ get; set; }              // 플레이어를 담는 리스트 , PhotonManager에서 Add시킴










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








    /************************************ ResultUI ***********************************************/


    /**** ResultUI ****/

    public GameObject ResultUI { set; get; }
    public void InitResultUI() { ResultUI = GameObject.Find("ResultUI"); }





    





    /********************** InGameCanvas ******************/
    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas() { InGameCanvas = GameObject.Find("InGameCanvas"); }
    


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
        uIManager = this;

        // 플레이어 리스트 초기화
        Players = new List<PhotonPlayer>();

        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();




        // UI 초기화들
        InitUICanvas();

        // UI 초기화들
        InitUICanvas();

        InitInGameCanvas();

        InitResultUI();



        InitBackgroundPanel();

        InitAimPanel();
        InitAimImage();




        limitTimePanelScript = new LimitTimePanelScript();
        limitTimePanelScript.InitData();

        getScoreImageScript = new GetScoreImageScript();
        getScoreImageScript.InitData();

        endStatePanelScript = new EndStatePanelScript();
        endStatePanelScript.InitData();


        // 스코어 패널 관련 내용 스크립트로 따로 처리
        hPPanelScript = new HPPanelScript();
        hPPanelScript.InitData();

        mPPanelScript = new MPPanelScript();
        mPPanelScript.InitData();

        scorePanelScript = new ScorePanelScript();
        scorePanelScript.InitData(MaxUISlot);

        timerBarPanelScript = new TimeBarPanelScript();
        timerBarPanelScript.InitData();

        helpPanelScript = new HelpPanelScript();
        helpPanelScript.InitData();

        starPanelScript = new StarPanelScript();
        starPanelScript.InitData();

        mouseImagePanelScript = new MouseImagePanelScript();
        mouseImagePanelScript.InitData();


    }


    private void Update()
    {

        // 하위 스크립트의 이벤트들을 실행
        UpdateEvent();



    }



    /**** UI 이벤트  ****/


    // UI 이벤트

    public void ClickScoreNextButton()
    {
        scorePanelScript.IsUseNextButton = true;
    }

}
