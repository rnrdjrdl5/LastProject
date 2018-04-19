﻿using System.Collections;
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
        HideCursor();

        // player들 나오면 PhotonManager에 추가한다. 
        PhotonManager photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        photonManager.AllPlayers.Add(gameObject);

    }
}
