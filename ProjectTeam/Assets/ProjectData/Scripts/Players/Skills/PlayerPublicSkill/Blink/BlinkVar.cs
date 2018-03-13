using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Blink
{
    private bool           isBlinkOn                = false;

    private GameObject     BlinkObject;

    private float          BlinkDistance           = 20.0f;
    private float          RaycastInfinity         = 10000.0f;

    public GameObject     PlayerCamera;



    void SetisBlinkOn(bool BO) { isBlinkOn = BO; }
    bool GetisBlinkOn() { return isBlinkOn; }


    /***** 아래부터는 인스펙터에 들어나는 변수입니다. *****/



    public GameObject       BlinkPrefab;

    
}
