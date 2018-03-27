using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 상호작용을 위한 스킬의 최상 부모입니다.
public partial class DefaultInteraction : Photon.PunBehaviour {

    // 1. 본인이고
    // 2. 상태값을 만족하고
    // 3. 키가 눌려야하고
    // 4. 상호작용 과의 거리가 멀지 않아야 한다.


        //첫 초기화로 각종 설정을 등록합니다.
    virtual protected void Awake()
    {
        pv = gameObject.GetComponent<PhotonView>();
        animator = gameObject.GetComponent<Animator>();
        fo = gameObject.GetComponent<FindObject>();

        InGameCanvas = GameObject.Find("InGameCanvas");

        // 타임바를 받습니다.
        TimeBarScript = gameObject.GetComponent<TimeBar>();

        // findobject 스크립트를 받습니다. 대상 탐지용입니다.
        FindObjectScript = gameObject.GetComponent<FindObject>();

        // 카메라 스크립트를 받습니다.
        PlayerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();



        // 오브젝트 넘기기 에 필요한 스크립트를 고릅니다.
        ChoosePushScript();

        // 오브젝트 넘기기에 필요한 정보를 등록합니다.
        InitPushObject();




    }

   

    virtual protected void Update()
    {

        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            // 상호작용이 가능한지 판단합니다. findobject에서 판단합니다.
            if (fo.GetIsInteraction())
            {

                // 상태 살피기.
                if (CheckState())
                {
                    // 키 눌렸는지?
                    if (CheckSkillKey())
                    {
                        // 상호작용 오브젝트 등록
                        InteractiveObject = FindObjectScript.GetObjectTarget();

                        //상호작용 탐지 스크립트 해제, 상호작용 ui 삭제
                        FindObjectScript.SetisUseFindObject(false);

                        // 상호작용의  상태를 처음으로 돌려놓습니다.
                        FindObjectScript.BackDefault();

                        //카메라 설정 변경
                        PlayerCamera.SetCameraRadX(-transform.eulerAngles.y);
                        PlayerCamera.SetCameraModeType(PlayerCamera.EnumCameraMode.FREE);

                        // 타임바 기본설정
                        BaseTimeBarScript();

                        // 애니메이션 재생
                        UseAnimation();

                        // 카메라의 기존 위치를 Push용 클래스에 보내줍니다.
                        defaultPushObject.SetOriginalCameraPosition(PlayerCamera.transform.position);

                        // 다른 클라이언트에 정보를 보냅니다.
                        pv.RPC("RPCCameraPosition", PhotonTargets.Others, PlayerCamera.transform.position);
                        // 해제조건
                        //    1. 애니메이션이 끝날 때
                        //    2. 플레이어가 이동했을 때

                    }

                }

            }

        }

    }




}
