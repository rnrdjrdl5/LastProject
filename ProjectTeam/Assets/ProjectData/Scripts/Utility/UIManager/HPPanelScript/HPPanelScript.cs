using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPPanelScript{

    /**** HP ****/

    public GameObject HPPanel { set; get; }
    public void InitHPPanel() { HPPanel = UIManager.GetInstance().UICanvas.transform.Find("HPPanel").gameObject; }

    public GameObject NowHpImage { set; get; }
    public Image NowHpImageImage { get; set; }

    private PlayerHealth playerHealth;

    // playerhealth에서 사용함.
    public void SetPlayerHealth(GameObject go)
    {
        playerHealth = go.GetComponent<PlayerHealth>();

        // PlayerHelath에 다시 등록함.
        playerHealth.HealthEvent += new PlayerHealth.HealthDele(HPEvent);
    }


    public void InitNowHPImage()
    {
        NowHpImage = HPPanel.transform.Find("NowHpImage").gameObject;
        NowHpImageImage = NowHpImage.GetComponent<Image>();
    }

    public void SetHealthPoint(bool isActive)
    {
        HPPanel.SetActive(isActive);
        NowHpImage.SetActive(isActive);
    }

    public void InitData()
    {
        InitHPPanel();
        InitNowHPImage();

        
    }

    public void HPEvent()
    {
            NowHpImageImage.fillAmount =
                playerHealth.GetNowHealth() / playerHealth.GetMaxHealth();

    }

}
