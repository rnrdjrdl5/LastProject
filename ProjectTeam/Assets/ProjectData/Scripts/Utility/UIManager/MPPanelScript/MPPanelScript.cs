using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MPPanelScript {


    public GameObject MPPanel { set; get; }
    public void InitMPPanel() { MPPanel = UIManager.GetInstance().UICanvas.transform.Find("MPPanel").gameObject; }

    public GameObject NowMpImage { set; get; }
    public void InitNowMPImage() { NowMpImage = MPPanel.transform.Find("NowMpImage").gameObject; }

    public Image NowMpImageImage { get; set; }
    public void InitNowMpImageImage() { NowMpImageImage = NowMpImage.GetComponent<Image>(); }

    public void SetManaPoint(bool isActive)
    {
        MPPanel.SetActive(isActive);
        NowMpImage.SetActive(isActive);
    }

    public PlayerManaPoint playerManaPoint;

    // playerhealth에서 사용함.
    public void SetPlayerMana(GameObject go)
    {
        playerManaPoint = go.GetComponent<PlayerManaPoint>();

        // PlayerHelath에 다시 등록함.
        playerManaPoint.ManaEvent += new PlayerManaPoint.ManaDele(MPEvent);
    }

    public void InitData()
    {
        InitMPPanel();

        InitNowMPImage();

        InitNowMpImageImage();
    }


    public void MPEvent()
    {
        
        Debug.Log("이벤트갱신");
        NowMpImageImage.fillAmount =
            playerManaPoint.GetNowManaPoint() / playerManaPoint.GetMaxManaPoint();
    }


}
