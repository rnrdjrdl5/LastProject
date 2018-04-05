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
        SetMPBar();
        SetTimerText();
        SetScore();



       // rt.position += Vector3.up * ( (Input.GetAxis("Mouse Y")) * Time.deltaTime * 30 );

    }

    // Update is called once per frame


}
