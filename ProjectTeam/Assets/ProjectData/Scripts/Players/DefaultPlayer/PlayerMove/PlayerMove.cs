using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMove : Photon.PunBehaviour
{
    private void Awake()
    {
        SetAwake();
    }

    // Use this for initialization
    void Start()
    {

    }


    bool UpMove = false;
    bool RightMove = false;
    // Update is called once per frame
    void Update()
    {


        

        PlayerMoveAnimation();

        PlayerTransform();


        if (UpMove)
            HSpeed = 1;

        if (RightMove)
            VSpeed = 1;
    }

    

}
