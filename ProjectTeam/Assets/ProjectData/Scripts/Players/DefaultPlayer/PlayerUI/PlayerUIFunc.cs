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
            MPBar = UIObject.transform.Find("HPMPPanel/MP_Bar").gameObject.GetComponent<Image>();

            Debug.Log(MPBar);

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

    void SetMPBar()
    {
        if(gameObject.GetComponent<PhotonView>().isMine)
        {
              MPBar.fillAmount =
                  GetComponent<PlayerManaPoint>().GetNowManaPoint() /
                  GetComponent<PlayerManaPoint>().GetMaxManaPoint();
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
