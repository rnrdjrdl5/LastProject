using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SinClass
{
    public static SinClass instance;
    public void Awake()
    {
        SinClass.instance = this;
    }

    private CollisionObjectDamage collisionObjectDamage;
    private CollisionDamagedDebuff collisionDamagedDebuff;
    private CollisionObject collisionObject;


}

public partial class PlayerManager
{
    
    void HideCursor()
    {
        // Mouse Lock

        Cursor.lockState = CursorLockMode.Locked;

        // Cursor visible

        Cursor.visible = false;
    }

    void SetFallowCamera()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            GameObject pc = GameObject.Find("PlayerCamera");
            pc.GetComponent<PlayerCamera>().PlayerObject = gameObject;
            pc.GetComponent<PlayerCamera>().isPlayerSpawn = true;
        }        

    }

    // Damage 들어갈 떄, 관련 데미지 모두삭제.





    
}
