using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerUI{

    public GameObject UIPrefab;         // UI 프리팹을 통해서 동적생성하기 위한 변수



    private GameObject UIObject;        // UI 프리팹을 통해 동적생성된 오브젝트를 지정하는 변수
    public GameObject GetUIObject(){return UIObject;}
    public void SetUIObject(GameObject G){UIObject = G;}


    private Image HPBar;                     // HP 이미지를 받아서 Amont를 조정하기 위한 변수   
    public Image GetHPBar() { return HPBar; }
    public void SetHPBar(Image hp) { HPBar = hp; }


    private Image MPBar;
    public Image GetMPBar() { return MPBar; }
    public void SetMPBar(Image i) { MPBar = i; }

    private GameObject PhotonManager;

    private Text TimerText;



}
