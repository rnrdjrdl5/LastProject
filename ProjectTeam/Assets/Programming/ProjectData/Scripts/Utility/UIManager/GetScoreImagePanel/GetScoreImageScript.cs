using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetScoreImageScript  : Behaviour{


    public GameObject GetScorePanel { get; set; }
    public void InitGetScorePanel() { GetScorePanel = UIManager.GetInstance().InGameCanvas.transform.Find("GetScorePanel").gameObject; }

    public void InitData()
    {
        InitGetScorePanel();
    }

}
