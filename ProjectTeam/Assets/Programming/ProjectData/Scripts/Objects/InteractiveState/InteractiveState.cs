﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveState : Photon.MonoBehaviour, IPunObservable {


    /**** 열거형 ****/

    // 물체의 타입
    public enum EnumInteractiveObject
    { TABLE = 1, MIKE, DRAWE, POT, PIANO, POSMEKA, };

    // 물체의 액션 적용 방식
    public enum EnumAction
    { PHYSICS, ANIMATION, MIX };

    // 물체가 애니메이션 일때, 상호작용 거리 타입 
    // 1. 특정위치    2. 특정 거리 
    public enum EnumInterPos
    { POSITION, DISTANCE, NONE };






    /*** public ****/


    [Header(" - 상호작용 시전시간")]
    [Tooltip(" - 상호작용이 완료되기 까지의 시간입니다.(게이지에 사용)")]
    public float InteractiveTime = 0.0f;


    public bool CanUseObject;                           // 사용자가 이미한번 뒤집었는지 파악하기 위한 용도.

    [Header(" - 오브젝트 타입")]
    [Tooltip(" - 오브젝트가 어떤 오브젝트인지 정한다.")]


    public EnumInteractiveObject interactiveObjectType;                  // 상호작용 오브젝트 타입

    [Header(" - 액션 타입")]
    [Tooltip(" - 어떤 액션인지 판단한다.")]
    public EnumAction ActionType;                             // 상호작용 액션 타입

    [Header(" - 점수")]
    [Tooltip(" - 점수 획득량")]
    public int InterObjectScore;

    [Header(" - 상호작용 플레이어 위치")]
    [Tooltip(" - 플레이어 위치 오브젝트")]
    public GameObject PlayerInterPosition;


    [Header(" - 상호작용 위치 타입")]
    [Tooltip(" - 상호작용 위치가 고정형인지 거리형인지 판단.")]
    public EnumInterPos InterPosType = EnumInterPos.NONE;

    [Header(" - 상호작용 먼지 이펙트 위치")]
    [Tooltip(" - 상호작용 이펙트 위치입니다.")]
    public GameObject InterEffect;




    public bool IsUseAction { get; set; }
    /**** private ****/


    public bool isCanFirstCheck;                                        // 플레이어가 상호작용을 누를 수 있는지 가능성
    private int playerViewID;                                           // 플레이어 스킬을 찾기 위한 id 저장용


    private NewInteractionSkill newInteractionSkill;                    // 플레이어 스킬을 가진 스크립트
    private ObjectManager objectManager;                // 플레이어 오브젝트 관리  매니저

    private bool isPlayerAction = false;                              // 해당 플레이어 액션 했는지 여부

    private Animator animator;

    public float InterPositionDis { get; set; }          // 플레이어 상호작용 거리

    public List<Material> InterMaterials; // 메테리얼 저장

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
    public void SetisCanFirstCheck(bool CFC) { isCanFirstCheck = CFC; }

    public List<Material> GetInterMaterials() { return InterMaterials; }









    // Use this for initialization

    private void Awake()
    {
        InterMaterials = new List<Material>();

        CanUseObject = true;        // 반투명 아닌 상태
        isCanFirstCheck = true;     // 첫 상호작용 사용 가능

        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        objectManager.AddInterObj(gameObject);

        animator = GetComponent<Animator>();

        IsUseAction = false;

        if (InterPosType != EnumInterPos.NONE)
            InterPositionDis = (PlayerInterPosition.transform.position - transform.position).magnitude;

        MeshRenderer[] mrs = gameObject.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < mrs.Length; i++)
        {
            InterMaterials.Add(mrs[i].material);
        }


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
         
        // 마스터가 관리한다.  동기화도 겸해서 같이해준다. 해당 값으로 반투명 체크함.
        if (PhotonNetwork.isMasterClient)
            CanUseObject = false;

        if (newInteractionSkill != null)
        {


            // 개인 점수 생성 
            // UIManager.GetInstance().CreateScoreImage(InterObjectScore);

            CreateScore();

            Debug.Log("asd");
       
            if (ActionType == EnumAction.PHYSICS)
            {

                //  스킬 을 사용한 플레이어인 경우                           ( 스킬 사용한 사람만 newinteractionSkill이 할당됨 ) 

                    photonView.RPC("RPCTableAction", PhotonTargets.All, NormalVector3);
                
            }


            // 애니메이션 인 경우
            else if (ActionType == EnumAction.ANIMATION)
            {


                    photonView.RPC("RPCAnimation", PhotonTargets.All);
                
            }
        }
    }

    public bool FindnterObjectMaterial(Material m)
    {
        

        for (int i = 0; i < InterMaterials.Count; i++)
        {
            if (InterMaterials[i].name == m.name)
                return true;

        }
        return false;
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


    public void CallActionAnimation()
    {
        photonView.RPC("RPCActionAnimation", PhotonTargets.All);
    }

    public void CallRPCCancelActionAnimation()
    {
        photonView.RPC("RPCCancelActionAnimation", PhotonTargets.All);
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

        // 플레이어와의 충돌은 없애고, 상호작용탐지도 없앤다.
        gameObject.layer = LayerMask.NameToLayer("NoPlayerIntering");

            // 물리 컴포넌트 받기
            TablePhysics tablePhysics = GetComponent<TablePhysics>();

            // 물리 방식 액션 사용
            tablePhysics.Action(normalVector3);
        //}
    }

    // 애니메이션용 RPC
    [PunRPC]
    private void RPCAnimation()
    {


        // 1. 리스트에서 해당 오브젝트 삭제
        objectManager.RemoveObject(photonView.viewID);

        // 충돌체크 삭제
        //Destroy(gameObject.GetComponent<BoxCollider>());

        // 3. 사용했다고 처리
        // ( 애니메이션에서 exit 되어도 물체 돌리지 않음 )  
        IsUseAction = true;
    }

    // 모든 대상
    // 상호작용물체 사용 끝났다고 알림 (true설정)
    [PunRPC]
    private void RPCOnCanFirstCheck()
    {
        isCanFirstCheck = true;
    }

    // 모든 대상
    // 상호작용 애니메이션 을 킨다.
    // 층돌 레이어를 변경한다.

        // 사용 초반
    [PunRPC]
    public void RPCActionAnimation()
    {
        // 충돌레이어 변경
        // 플레이어와의 충돌은 없애고, 상호작용탐지도 없앤다.
        gameObject.layer = LayerMask.NameToLayer("NoPlayerIntering");

        animator.SetBool("isAction", true);

    }

    // 모든 대상
    // 상호작용이 끊겼으므로 애니메이션을 끈다.
    // 레이어도 돌려준다.
    [PunRPC]
    public void RPCCancelActionAnimation()
    {
        // 충돌레이어 변경
        gameObject.layer = LayerMask.NameToLayer("MainObject");
        animator.SetBool("isAction", false);
    }



    /**** 애니메이션 이벤트 *****/
    public void BigFallDown()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.BIGDUST_BIG);
        go.transform.position = InterEffect.transform.position;

        go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.BIGDUST_SMALL);
        go.transform.position = InterEffect.transform.position;
    }


    public void MiddleFallDown()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIDDLEDUST_BIG);
        go.transform.position = InterEffect.transform.position;

        go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.MIDDLEDUST_SMALL);
        go.transform.position = InterEffect.transform.position;
    }

    public void SmallFallDown()
    {
        GameObject go = PoolingManager.GetInstance().CreateEffect(PoolingManager.EffctType.SMALL_DUST_SMALL);
        go.transform.position = InterEffect.transform.position;
    }

    public void InterAniSetLayer()
    {

        gameObject.layer = LayerMask.NameToLayer("NoPlayerInterEnd");
    }


    public void CreateScore()
    {
        GameObject getScoreText = PoolingManager.GetInstance().PopObject("GetScoreText");

        //2. 부모 설정
        getScoreText.transform.SetParent(UIManager.GetInstance().getScoreImageScript.GetScorePanel.transform);

        //3. 함수 실행
        getScoreText.GetComponent<GetScoreText>().CreateScore(InterObjectScore);
    }
}

