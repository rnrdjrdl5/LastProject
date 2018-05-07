using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanelScript{


    /**** HelperUI , HelpUI  , Helper : Icon ****/


    public bool isCanUseHelperUI = true;

    public GameObject HelperUIPanel { set; get; }
    public void InitHelperUIPanel() { HelperUIPanel = UIManager.GetInstance().UICanvas.transform.Find("HelperUIPanel").gameObject; }
    public GameObject HelpUIPanel { set; get; }
    public void InitHelpUIPanel() { HelpUIPanel = UIManager.GetInstance().UICanvas.transform.Find("HelpUIPanel").gameObject; }



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



    public void InitData()
    {

        InitHelperUIPanel();
        InitHelperUIImage();

        InitHelpUIPanel();
        InitCatHelpUIImage();
        InitMouseHelpUIImage();


        // 이벤트 등록
        UIManager.GetInstance().UpdateEvent += CheckShowHelpUI;


    }

    public void CheckShowHelpUI()
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
      
}
