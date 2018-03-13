using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerUI : Photon.PunBehaviour
{

    // Use this for initialization
    void Start () {
        SetUI();
        

    }

    private void Update()
    {
        SetHPBar();
        SetTimerText();

    }

    // Update is called once per frame


}
