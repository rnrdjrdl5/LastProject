using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class PlayerManager
{

    void SetPhotonManager()
    {
        
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    void HideCursor()
    {
        // Mouse Lock
        if (gameObject.GetPhotonView().isMine)
        {

            Cursor.lockState = CursorLockMode.Locked;

            // Cursor visible

            Cursor.visible = false;
        }
    }

    void SetFallowCamera()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            GameObject pc = GameObject.Find("PlayerCamera");
            pc.GetComponent<PlayerCamera>().PlayerObject = gameObject;
            pc.GetComponent<PlayerCamera>().isPlayerSpawn = true;
            pc.GetComponent<PlayerCamera>().SetPlayerMove(gameObject.GetComponent<PlayerMove>());
        }        

    }

    void AttachThisPlayer()
    {
        if (photonManager != null)
        {
            photonManager.AddAllPlayer(gameObject);
        }
    }

    // Damage 들어갈 떄, 관련 데미지 모두삭제.






}
