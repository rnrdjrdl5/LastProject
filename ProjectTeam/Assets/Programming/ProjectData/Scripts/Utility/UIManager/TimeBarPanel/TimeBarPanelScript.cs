using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarPanelScript {


    /** 캔버스들 ***/
    public GameObject InGameCanvas { get; set; }
    public void InitInGameCanvas() { InGameCanvas = UIManager.GetInstance().InGameCanvas; }

    public GameObject TimeBarPanel { get; set; }
    public void InitTimeBarPanel() { TimeBarPanel = InGameCanvas.transform.Find("TimeBarPanel").gameObject; }

    public Image MaxTimeBar { get; set; }

    public void InitMaxTimeBar() { MaxTimeBar = TimeBarPanel.transform.Find("MaxTimeBar").GetComponent<Image>(); }

    public Image NowTimeBar { get; set; }

    public void InitNowTimeBar() { NowTimeBar = TimeBarPanel.transform.Find("NowTimeBar").GetComponent<Image>(); }



    /*** 수치들 ***/
    public float MaxTimeBarTimer { get; set; }
    public float NowTimerBarTimer { get; set; }

    public bool IsUseTimerBar { get; set; }





    public void PreTimeBar(float _MaxTimer)
    {
        NowTimeBar.fillAmount = 0.0f;

        MaxTimeBarTimer = _MaxTimer;
        NowTimerBarTimer = 0.0f;

        TimeBarPanel.SetActive(true);
        IsUseTimerBar = true;
    }

    public void DestroyTimebar()
    {
        
        NowTimeBar.fillAmount = 0.0f;

        MaxTimeBarTimer = 0.0f;
        NowTimerBarTimer = 0.0f;

        TimeBarPanel.SetActive(false);
        IsUseTimerBar = false;
    }

    public void InitData()
    {
        InitInGameCanvas();
        InitTimeBarPanel();
        InitMaxTimeBar();
        InitNowTimeBar();

        UIManager.GetInstance().UpdateEvent += UpdateTimeBar;
    }

    public void UpdateTimeBar()
    {
        if (IsUseTimerBar)
        {

            if (NowTimerBarTimer >= MaxTimeBarTimer)
            {
                DestroyTimebar();
            }

            NowTimerBarTimer += Time.deltaTime;

            NowTimeBar.fillAmount =
                NowTimerBarTimer / MaxTimeBarTimer;
        }
    }



}
