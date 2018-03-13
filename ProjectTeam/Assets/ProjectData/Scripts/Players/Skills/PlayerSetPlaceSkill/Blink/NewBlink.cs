using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlink : DefaultSetPlaceSkill , IPunObservable {

    protected override void Awake()
    {
        base.Awake();
        PlayerUsingSkill = PlayerState.ConditionEnum.BLINK;
    }

    public override bool CheckState()
    {
        if ((EqualState(PlayerState.ConditionEnum.RUN) ||
              EqualState(PlayerState.ConditionEnum.IDLE)))
        {
            return true;
        }
        return false;
    }

    public override void UseSkill()
    {
        transform.position = PlaceVector;
        gameObject.GetComponent<PhotonView>().RPC("BlinkPlace", PhotonTargets.Others, transform.position);
    }

    // Update is called once per frame

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


    // 아래부터는 RPC함수입니다. 



    [PunRPC]
    void BlinkPlace(Vector3 newPosition)
    {
        transform.position = newPosition;
        gameObject.GetComponent<PlayerMove>().SetRecvPosition(newPosition);


        // 시리얼라이즈에서 이미 받고 나서 
        // update 로 가기 전에
        // RPC가 들어오면,
        // RPC로 인한 위치이동 > update로 인한 보간  이 순서대로 적용되서
        // RPC로 위치 이동했다가 보간위치로 이동함.
        // 그래서 시리얼라이즈로 받는 값도 바꿔준다.
        //     RecvPosition = newPosition;

        // 도착 후 생성? Instantiate(BlinkEffect, transform.position, Quaternion.identity);
    }

}
