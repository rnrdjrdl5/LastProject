using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerUI
{



    void SetUI()
    {
        if(gameObject.GetComponent<PhotonView>().isMine)
        {
            UIObject = Instantiate(UIPrefab) as GameObject;
            GameObject.Find("PhotonManager").GetComponent<PhotonManager>().UICanvas = UIObject;
            HPBar = UIObject.transform.Find("HPMPPanel/HP_Bar").gameObject.GetComponent<Image>();

            PhotonManager = GameObject.Find("PhotonManager");

            TimerText = UIObject.transform.Find("TimePanel/TimeValue").gameObject.GetComponent<Text>();
        }

    }

    void SetHPBar()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            HPBar.fillAmount =
                GetComponent<PlayerHealth>().GetNowHealth() /
                GetComponent<PlayerHealth>().GetMaxHealth();
        }
    }

    void SetTimerText()
    {
        if(gameObject.GetComponent<PhotonView>().isMine)
        {
            TimerText.text = Mathf.Floor(PhotonManager.GetComponent<PhotonManager>().GetTimeCount()).ToString();
        }
    }
}
