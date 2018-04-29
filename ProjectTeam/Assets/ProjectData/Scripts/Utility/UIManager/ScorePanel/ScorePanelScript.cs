using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanelScript{


    //설정한 최대 UI 개수 , 총 플레이할 수 있는 인원.
    public int MaxUISlot { get; set; }
    // UI 사용 가능한지 확인 
    public bool IsUseScoreUI { get; set; }


    public GameObject ScorePanel { get; set; }



    public GameObject ScoreDefaultPanel { get; set; }



    public GameObject ScorePlayerSlotPanel { get; set; }
    public GameObject[] PlayerSlot { get; set; }



    public GameObject ScorePlayerIconPanel { get; set; }
    public GameObject[] PlayerMouseImage { get; set; }
    public GameObject[] PlayerCatImage { get; set; }


    public GameObject ScoreNamePanel { get; set; }
    public Text[] PlayerName { get; set; }

    public GameObject ScoreMousePanel { get; set; }
    public Text[] PlayerMouseScore { get; set; }

    public GameObject ScoreCatPanel { get; set; }
    public Text[] PlayerCatScore { get; set; }

    public GameObject ScoreTotalPanel { get; set; }
    public Text[] PlayerTotalScore { get; set; }


    /***** 추가적으로 본인 플레이어 슬롯 색상 변경을 위해 image 컴포넌트 받아옴. ******/
    public Image[] PlayerSlotImage;



    public void InitData(int maxUISlot)
    {

        IsUseScoreUI = true;

        MaxUISlot = maxUISlot;

        Debug.Log(MaxUISlot);

        GameObject resultUI = GameObject.Find("ResultUI");
        ScorePanel = resultUI.transform.Find("ScorePanel").gameObject;

        ScoreDefaultPanel = ScorePanel.transform.Find("ScoreDefaultPanel").gameObject;





        ScorePlayerSlotPanel = ScorePanel.transform.Find("ScorePlayerSlotPanel").gameObject;

        PlayerSlot = new GameObject[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {

            PlayerSlot[i] = ScorePlayerSlotPanel.transform.Find("PlayerSlot" + (i + 1).ToString()).gameObject;
        }

        




        ScorePlayerIconPanel = ScorePanel.transform.Find("ScorePlayerIconPanel").gameObject;

        PlayerMouseImage = new GameObject[MaxUISlot];
        PlayerCatImage = new GameObject[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {

            PlayerMouseImage[i] = ScorePlayerIconPanel.transform.Find("PlayerMouseImage" + (i + 1).ToString()).gameObject;
            PlayerCatImage[i] = ScorePlayerIconPanel.transform.Find("PlayerCatImage" + (i + 1).ToString()).gameObject;
        }





        ScoreNamePanel = ScorePanel.transform.Find("ScoreNamePanel").gameObject;

        PlayerName = new Text[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {
            PlayerName[i] = ScoreNamePanel.transform.Find("PlayerName" + (i + 1).ToString()).gameObject.GetComponent<Text>();
        }





        ScoreMousePanel = ScorePanel.transform.Find("ScoreMousePanel").gameObject;

        PlayerMouseScore = new Text[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {
            PlayerMouseScore[i] = ScoreMousePanel.transform.Find("PlayerMouseScore" + (i + 1).ToString()).gameObject.GetComponent<Text>();
        }


        ScoreCatPanel = ScorePanel.transform.Find("ScoreCatPanel").gameObject;

        PlayerCatScore = new Text[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {
            PlayerCatScore[i] = ScoreCatPanel.transform.Find("PlayerCatScore" + (i + 1).ToString()).gameObject.GetComponent<Text>();
        }


        ScoreTotalPanel = ScorePanel.transform.Find("ScoreTotalPanel").gameObject;

        PlayerTotalScore = new Text[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {
            PlayerTotalScore[i] = ScoreTotalPanel.transform.Find("PlayerTotalScore" + (i + 1).ToString()).gameObject.GetComponent<Text>();
        }





        /****** 추가적으로 플레이어 슬롯 색상 변경을 위해 실시. *******/
        PlayerSlotImage = new Image[MaxUISlot];
        for (int i = 0; i < MaxUISlot; i++)
        {
            PlayerSlotImage[i] = PlayerSlot[i].GetComponent<Image>();
        }

    }

    public void ShowScorePanel(bool isActive)
    {

        ScorePanel.SetActive(isActive);
    }





    public void UpdateCatPlayerIcon()
    {

        for (int i = 0; i < MaxUISlot; i++)
        {
            if (i < PhotonNetwork.playerList.Length)
            {

                if ((string)UIManager.GetInstance().Players[i].CustomProperties["PlayerType"] == "Cat")
                {

                    PlayerCatImage[i].SetActive(true);
                    PlayerMouseImage[i].SetActive(false);
                }
                else
                {

                    PlayerCatImage[i].SetActive(false);
                    PlayerMouseImage[i].SetActive(true);
                }


            }

            else
            {
                PlayerCatImage[i].SetActive(false);
                PlayerMouseImage[i].SetActive(false);
            }

        }

    
    }

    public void UpdatePlayerSlot()
    {

        for (int i = 0; i < MaxUISlot; i++)
        {
            if (i < PhotonNetwork.playerList.Length)
                PlayerSlot[i].SetActive(true);

            else
                PlayerSlot[i].SetActive(false);
        }
    }

    public void UpdateIconSlots()
    {

        UpdateCatPlayerIcon();
        UpdatePlayerSlot();
    }

    public void UpdateMineSlot()
    {
        // 버그, 색상 변경안됨.
       
    }



    public void UpdatePlayerName()
    {

        for (int i = 0; i < MaxUISlot; i++)
        {
            if (i < PhotonNetwork.playerList.Length)
            {
                PlayerName[i].text = UIManager.GetInstance().Players[i].NickName;

            }
            else
            {
                PlayerName[i].text = "";
            }

        }
    }

    public void UpdateMouseScore()
    {

        for (int i = 0; i < MaxUISlot; i++)
        {
            if (i < PhotonNetwork.playerList.Length)
            {
                PlayerMouseScore[i].text = UIManager.GetInstance().Players[i].CustomProperties["MouseScore"].ToString();

            }
            else
            {
                PlayerName[i].text = "";
            }

        }
    }

    public void UpdateCatScore()
    {

        for (int i = 0; i < MaxUISlot; i++)
        {
            if (i < PhotonNetwork.playerList.Length)
            {
                PlayerCatScore[i].text = UIManager.GetInstance().Players[i].CustomProperties["CatScore"].ToString();

            }
            else
            {
                PlayerName[i].text = "";
            }

        }
    }

    public void UpdateTotalScore()
    {

        for (int i = 0; i < MaxUISlot; i++)
        {
            if (i < PhotonNetwork.playerList.Length)
            {
                PlayerTotalScore[i].text =
                    ((int)UIManager.GetInstance().Players[i].CustomProperties["CatScore"] +
                    (int)UIManager.GetInstance().Players[i].CustomProperties["MouseScore"]).ToString();

            }
            else
            {
                PlayerName[i].text = "";
            }

        }
    }



    public void UpdateScores()
    {
        UpdatePlayerName();
        UpdateMouseScore();
        UpdateCatScore();
        UpdateTotalScore();
        UpdateIconSlots();

        UpdateMineSlot();
    }

}
