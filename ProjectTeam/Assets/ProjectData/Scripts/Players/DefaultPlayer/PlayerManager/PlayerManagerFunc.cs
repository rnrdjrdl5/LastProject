﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class PlayerManager
{



    void SetFallowCamera()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            // playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
            playerCamera = PlayerCamera.GetInstance();
            playerCamera.PlayerObject = gameObject;
            playerCamera.SetPlayerMove(gameObject.GetComponent<PlayerMove>());

        }        

    }


    // Damage 들어갈 떄, 관련 데미지 모두삭제.






}
