using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class PlayerManager
{



    void SetFallowCamera()
    {
        // 연속적인 코드보다, if의 효율성을 생각
        // 모든 조건을 판단해서 if로 넣는게 아닌 느낌
        // 조건문 간결화신경쓰고
        // if else가 많아지면 팩토리로 빼버리기
        
        PhotonView pv = gameObject.GetComponent<PhotonView>();

        if (pv == null)
            return;

        if (!gameObject.GetComponent<PhotonView>().isMine)
            return;


        
        playerCamera = PlayerCamera.GetInstance();

        if (playerCamera == null)
            return;

        playerCamera.PlayerObject = gameObject;
        playerCamera.SetPlayerMove(gameObject.GetComponent<PlayerMove>());


    }


    // Damage 들어갈 떄, 관련 데미지 모두삭제.






}
