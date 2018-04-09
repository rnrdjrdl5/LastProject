using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UIManager : MonoBehaviour {
    /**** Type ****/
    public enum ResultType { BREAK,KILL,TIMEOVER }



    /**** UICanvas ****/
    public GameObject UICanvas { set; get; }

    public GameObject LimitTimePanel { set; get; }
    public GameObject HPPanel { set; get; }
    public GameObject MPPanel { set; get; }
    public GameObject StartRunPanel { set; get; }

    public GameObject LimitTimeText { set; get; }
    public Text LimitTimeTextText { set; get; }
    public GameObject NowHpImage { set; get; }
    public GameObject NowMpImage { set; get; }
    public GameObject StartRunText { set; get; }
    public Text StartRunTextText { set; get; }

    /**** ResultUI ****/
    public GameObject ResultUI { set; get; }

    public GameObject TimeWatchPanel { set; get; }
    public GameObject EndStatePanel { set; get; }

    public GameObject AllBreakImage { set; get; }
    public GameObject AllKillImage { set; get; }
    public GameObject TimeOverImage { set; get; }

   




    /**** InGameCanvas ****/


    // 초기화
    public void InitUICanvas()
    {
        UICanvas = GameObject.Find("UICanvas");

        LimitTimePanel = UICanvas.transform.Find("LimitTimePanel").gameObject;
        HPPanel = UICanvas.transform.Find("HPPanel").gameObject;
        MPPanel = UICanvas.transform.Find("MPPanel").gameObject;

        LimitTimeText = LimitTimePanel.transform.Find("LimitTimeText").gameObject;
        NowHpImage = HPPanel.transform.Find("NowHpImage").gameObject;
        NowMpImage = MPPanel.transform.Find("NowMpImage").gameObject;

        LimitTimeTextText = LimitTimeText.GetComponent<Text>();
    }

    // Result 초기화
    public void InitResultUI()
    {
        ResultUI = GameObject.Find("ResultUI").gameObject;

        TimeWatchPanel = ResultUI.transform.Find("TimeWatchPanel").gameObject;
        EndStatePanel = ResultUI.transform.Find("EndStatePanel").gameObject;


        AllBreakImage = EndStatePanel.transform.Find("AllBreakImage").gameObject;
        AllKillImage = EndStatePanel.transform.Find("AllKillImage").gameObject;
        TimeOverImage = EndStatePanel.transform.Find("TimeOverImage").gameObject;
    }

    public void InitStartRunText()
    {
        StartRunPanel = UICanvas.transform.Find("StartRunPanel").gameObject;
        StartRunText = StartRunPanel.transform.Find("StartRunText").gameObject;

        StartRunTextText = StartRunText.GetComponent<Text>();
    }

    // Active 설정
    public void SetUICanvas(bool isActive)
    {
        //UICanvas.SetActive(isActive);

        LimitTimePanel.SetActive(isActive);
        HPPanel.SetActive(isActive);
        MPPanel.SetActive(isActive);

        LimitTimeText.SetActive(isActive);
        NowHpImage.SetActive(isActive);
        NowMpImage.SetActive(isActive);

    }

    // Result 타입 설정
    public void SetResultUI(bool isActive , ResultType rT)
    {
        //ResultUI.SetActive(isActive);

        TimeWatchPanel.SetActive(isActive);
        EndStatePanel.SetActive(isActive);

        AllBreakImage.SetActive(false);
        AllKillImage.SetActive(false);
        TimeOverImage.SetActive(false);

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
    }

    // StartRun 설정
    public void SetStartRunUI(bool isActive)
    {
        StartRunPanel.SetActive(isActive);
        StartRunText.SetActive(isActive);
    }
    /**** 유니티 함수 ****/
    private void Awake()
    {
        InitUICanvas();
        InitResultUI();
        InitStartRunText();
    }

}
