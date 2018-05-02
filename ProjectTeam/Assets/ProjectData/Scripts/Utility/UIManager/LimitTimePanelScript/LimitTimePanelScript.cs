using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitTimePanelScript{
    /**** LimitTime ****/

    public GameObject LimitTimePanel { set; get; }
    public void InitLimitTimePanel() { LimitTimePanel = UIManager.GetInstance().UICanvas.transform.Find("LimitTimePanel").gameObject; }

    public GameObject LimitTimeText { set; get; }
    public void InitLimitTimeText() { LimitTimeText = LimitTimePanel.transform.Find("LimitTimeText").gameObject; }

    public Text LimitTimeTextText { set; get; }
    public void InitLimitTimeTextText() { LimitTimeTextText = LimitTimeText.GetComponent<Text>(); }

    public void SetLimitTime(bool isActive)
    {
        LimitTimePanel.SetActive(isActive);
        LimitTimeText.SetActive(isActive);
    }

    public void InitData()
    {
        InitLimitTimePanel();

        InitLimitTimeText();

        InitLimitTimeTextText();
    }
}
