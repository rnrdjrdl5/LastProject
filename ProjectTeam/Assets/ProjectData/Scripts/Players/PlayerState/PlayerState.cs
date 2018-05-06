using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerState : MonoBehaviour {


    public float ShakeTime = 0.5f;
    public float ShakeTick = 0.1f;
    public float ShakePower = 0.3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        SetHeadObject();

        PlayerType = (string)PhotonNetwork.player.CustomProperties["PlayerType"];

    }

    private void Update()
    {

    }
    
}
