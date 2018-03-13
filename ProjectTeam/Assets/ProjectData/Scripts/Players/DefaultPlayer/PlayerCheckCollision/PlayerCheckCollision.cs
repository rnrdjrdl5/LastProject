using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코더 : 반재억
// 제작일 : 2018. 02. 22
// 사용목적 : 플레이어 충돌체크를 담당.
// 사용하는 곳 : 플레이어 컴포넌트.
public class PlayerCheckCollision : Photon.PunBehaviour , IPunObservable {

    private BaseCollision baseCollision;

	// Use this for initialization
	void Start () {
        baseCollision = gameObject.AddComponent<BaseCollision>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnTriggerEnter(Collider other)
    {
        //CheckCollision(gameObject, other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckCollision(gameObject, other);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


    // 클라이언트, 서버 모두 실시합니다.
    void CheckCollision(GameObject go , Collider other)
    {
          if (other.tag == "CheckCollision")
          {
             // other : 충돌체크용 오브젝트, 
             // other에 있는 변수를 보고, 상태이상에 대해 판단한다.
             // ex) if other.getcomponent<Bullet>().getType() == "슬로우"
             //      bullet : 
              baseCollision.UseCollision(other);
             
          }

        if(other.tag == "DeadZone")
        {
            if (gameObject.GetComponent<PhotonView>().isMine)
            {
                if (other.tag == "DeadZone")
                {
                    gameObject.GetComponent<PlayerHealth>().PlayerDead();
                    Debug.Log("플레이어 데드존 입장");


                }
            }
        }

          
    }
}
