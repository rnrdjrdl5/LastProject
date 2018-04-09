using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class PlayerManager
{

    public void SetPhotonManager()
    {    
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }
    public PhotonManager GetPhotonManager() { return photonManager; }


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
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
            playerCamera.PlayerObject = gameObject;
            playerCamera.isPlayerSpawn = true;
            playerCamera.SetPlayerMove(gameObject.GetComponent<PlayerMove>());

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
