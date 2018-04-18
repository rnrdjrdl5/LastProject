﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveState : Photon.MonoBehaviour , IPunObservable {

    /**** 열거형 ****/

    // 물체의 타입
    public            enum           EnumInteractiveObject
    { TABLE  = 1 , FLOWERPOT};

    // 물체의 액션 적용 방식
    public            enum           EnumAction
    { PHYSICS, ANIMATION , MIX};


  


    /*** public ****/


    [Header(" - 상호작용 시전시간")]
    [Tooltip(" - 상호작용이 완료되기 까지의 시간입니다.(게이지에 사용)")]
    public           float                          InteractiveTime             = 0.0f; 
    public           bool                           CanUseObject;                           // 사용자가 이미한번 뒤집었는지 파악하기 위한 용도.


    public           EnumInteractiveObject          interactiveObjectType;                  // 상호작용 오브젝트 타입
    public           EnumAction                     ActionType;                             // 상호작용 액션 타입


    /**** private ****/


    public          bool           isCanFirstCheck;                                        // 플레이어가 상호작용을 누를 수 있는지 가능성
    private          int            playerViewID;                                           // 플레이어 스킬을 찾기 위한 id 저장용


    private          NewInteractionSkill            newInteractionSkill;                    // 플레이어 스킬을 가진 스크립트
    private ObjectManager objectManager;                // 플레이어 오브젝트 관리  매니저

    private         bool            isPlayerAction = false;                              // 해당 플레이어 액션 했는지 여부

    


    /**** 접근자 ****/


    public bool GetCanUseObject()
    {
        return CanUseObject;
    }
    public void SetCanUseObject(bool s)
    {
        CanUseObject = s;
    }

    public bool GetisCanFirstCheck() { return isCanFirstCheck; }
    public void SetisCanFirstCheck(bool CFC){isCanFirstCheck = CFC;}






    // Use this for initialization

    private void Awake()
    {
        CanUseObject = true;        // 반투명 아닌 상태
        isCanFirstCheck = true;     // 첫 상호작용 사용 가능

        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        objectManager.AddInterObj(gameObject);

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
            if (stream.isWriting)
            {
                stream.SendNext(CanUseObject);
                stream.SendNext(isCanFirstCheck);
            }

            if(stream.isReading)
            {   
                CanUseObject = (bool)stream.ReceiveNext();
                isCanFirstCheck = (bool)stream.ReceiveNext();
            }
    }


    // 액션을 사용함
    // 물리 인 경우 카메라의 위치 이용, 매개변수 받음

    // 개인적으로 모두 사용
    public void UseAction(Vector3 NormalVector3)
    {

        if (PhotonNetwork.isMasterClient)
            CanUseObject = false;

        if (ActionType == EnumAction.PHYSICS)
        {

            //  스킬 을 사용한 플레이어인 경우                           ( 스킬 사용한 사람만 newinteractionSkill이 할당됨 ) 

            if (newInteractionSkill != null)
            {
                if (newInteractionSkill.photonView.isMine)
                {
                    // 개인 점수 생성 
                    GameObject GetScoreImageObject = Instantiate(PoolingManager.GetInstance().GetScoreImage);

                    Debug.Log(GetScoreImageObject);
                    Debug.Log(UIManager.GetInstance().InGameCanvas.transform.Find("GetScorePanel"));
                    GetScoreImageObject.transform.SetParent(UIManager.GetInstance().InGameCanvas.transform.Find("GetScorePanel"));

                    GetScoreImageObject.transform.localScale = Vector3.one;


                    Vector3 v3 = new Vector3{x = Screen.width * 0.7f,y = Screen.height* 0.55f,z = 0.0f};

                    GetScoreImageObject.transform.position = v3;

                    Destroy(GetScoreImageObject, 3.0f);

                    photonView.RPC("RPCTableAction", PhotonTargets.All, NormalVector3);
                }
            }
            /*// 충돌체크 변경
            gameObject.layer = LayerMask.NameToLayer("NoCollisionPlayer");

            // 물리 컴포넌트 받기
            TablePhysics tablePhysics = GetComponent<TablePhysics>();

            // 물리 방식 액션 사용
            tablePhysics.Action(NormalVector3);*/


        }
        /*
        if (ActionType == EnumAction.ANIMATION ||
          ActionType == EnumAction.MIX)
        {

        }*/

        //  }
    }



    /**** RPC를 부르는 함수입니다. ****/

    // 상호작용 스킬에서 물체에게 보낸 메세지
    public void CallInterMessage(int vID , NewInteractionSkill nis)
    {

        // 플레이어 스킬을 가리킨다.
        newInteractionSkill = nis;

        // 함수 사용한 플레이어 ViewID 저장
        playerViewID = vID;

        // 마스터 클라이언트에 메시지 보내서 상호작용 가능한지 파악
        photonView.RPC("RPCInterMessage", PhotonTargets.All, vID);
    }

    // 클라이언트가 모든 클라이언트들에게 알림.
    public void CallOnCanFirstCheck()
    {
        photonView.RPC("RPCOnCanFirstCheck", PhotonTargets.All);
    }

    /**** RPC입니다. ****/

    // 모든대상 + 조건부 마스터인경우
    // 모든 클라이언트 상호작용 물체가 받음.
    [PunRPC]
    private void RPCInterMessage(int vID)
    {

        // 모든 클라이언트는 상호작용 물체를 알고 있어야 함.
        

        // 마스터인 경우
        if (PhotonNetwork.isMasterClient)
        {


            // 첫 체크가 가능하다면
            if (isCanFirstCheck == true)
            {
                Debug.Log("체크불가설정 , + " + photonView.viewID);


                // 체크가 불가능하도록 변경
                isCanFirstCheck = false;

                // RPC 이용해서 스킬 스크립트 찾음
                // 다른 클라이언트들도 체크 불가능하도록 변경
                photonView.RPC("RPCReturnSkill", PhotonTargets.All, vID);


            }


            // 첫 체크가 불가능하다면
            else
            {
                // 이미 사용중이라고 알립니다.

                // 플레이어의 상태를 원래대로 돌려놓습니다.
                photonView.RPC("RPCRetunDontSkill", PhotonTargets.All, vID);
            }
        }
    }

    // 모든 대상
    // 사용 허가를 내려주는 UseSkill 메세지 전송
    [PunRPC]
    private void RPCReturnSkill(int vID)
    {

        // 스킬 사용자 찾기
        if(vID == playerViewID)
        {
            newInteractionSkill.UseSkill();
        }

        else
        {
            // 해당 오브젝트를 찾습니다.
        }
    }

    // 모든 대상
    // 스킬 사용 불가 설정
    [PunRPC]
    private void RPCRetunDontSkill(int vID)
    {

        // 스킬 사용자 찾기
        if (vID == playerViewID)
        {
            newInteractionSkill.DontUseSkill();
        }

    }

    // 물리 용 RPC
    // 무사히 상호작용 되었는지판단
    // 안되있으면 상호작용 적용
    [PunRPC]
    private void RPCTableAction(Vector3 normalVector3)
    {
        // playeraction 한 적이 없다면.
        /*   if (isPlayerAction == false)
           {
               isPlayerAction = true;*/

        // 리스트에서 해당 오브젝트 삭제
        objectManager.RemoveObject(photonView.viewID);

            // 충돌체크 변경
            gameObject.layer = LayerMask.NameToLayer("NoCollisionPlayer");

            // 물리 컴포넌트 받기
            TablePhysics tablePhysics = GetComponent<TablePhysics>();

            // 물리 방식 액션 사용
            tablePhysics.Action(normalVector3);
        //}
    }

    // 모든 대상
    // 상호작용물체 사용 끝났다고 알림 (true설정)
    [PunRPC]
    private void RPCOnCanFirstCheck()
    {
        isCanFirstCheck = true;
    }

    // 모든 대상
    // 플레이어와 사물간의 충돌체크를 없앤다.
}

