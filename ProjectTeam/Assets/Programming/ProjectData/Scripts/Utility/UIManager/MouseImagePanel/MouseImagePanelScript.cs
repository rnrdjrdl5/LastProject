using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseImagePanelScript  {

    public int MaxMouseImage = 5;

    /**** MouseImagePanel ****/
    public GameObject MouseImagePanel;
    public void InitMouseImagePanel() { MouseImagePanel = UIManager.GetInstance().UICanvas.transform.Find("MouseImagePanel").gameObject; }

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

    public void UpdateNowMouseImage()
    {
        // 0. 현재 접속유저 판단.

        int MouseAmount = PhotonNetwork.playerList.Length - 1;

        for (int i = 0; i < MaxMouseImage; i++)
        {


            /* if (i < MouseAmount)
             {
                 MouseOffImage[i].SetActive(false);
                 MouseImage[i].SetActive(false);
             }*/
            MouseOffImage[i].SetActive(false);
            MouseImage[i].SetActive(false);
        }


        // 쥐 수만큼 추가
        int NowLiveMouse = 0;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if ((string)PhotonNetwork.playerList[i].CustomProperties["PlayerType"] == "Mouse")
            {

                // 1. 쥐면 추가한다.
                NowLiveMouse++;
            }

        }


        // 쥐 플레이어만큼.
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

    public void InitData()
    {
        InitMouseImagePanel();
        InitMouseImage();
        InitMouseOffImage();

        UIManager.GetInstance().UpdateEvent += UpdateNowMouseImage;
    }
}
