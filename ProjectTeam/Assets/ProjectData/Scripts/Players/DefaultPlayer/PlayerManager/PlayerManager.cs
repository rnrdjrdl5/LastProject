using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerManager : MonoBehaviour {

    

    // Use this for initialization
    private void Awake()
    {
        SetFallowCamera();


    }
    private void Start()
    {
        SetPhotonManager();
        HideCursor();

        AttachThisPlayer();
    }
}
