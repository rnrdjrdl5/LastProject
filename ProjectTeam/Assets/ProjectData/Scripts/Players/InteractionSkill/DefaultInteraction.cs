using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DefaultInteraction : Photon.PunBehaviour {

    // 1. 본인이고
    // 2. 상태값을 만족하고
    // 3. 키가 눌려야하고
    // 4. 상호작용 과의 거리가 멀지 않아야 한다.

    virtual protected void Awake()
    {
        pv = gameObject.GetComponent<PhotonView>();
        animator = gameObject.GetComponent<Animator>();
        fo = gameObject.GetComponent<FindObject>();

        InGameCanvas = GameObject.Find("InGameCanvas");


        TimeBarScript = gameObject.GetComponent<TimeBar>();

        FindObjectScript = gameObject.GetComponent<FindObject>();

        PlayerCamera = GameObject.Find("PlayerCamera");

        


    }



    virtual protected void Update()
    {

        if (gameObject.GetComponent<PhotonView>().isMine)
        {
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
                        FindObjectScript.SetisFindObject(false);
                        FindObjectScript.BackDefault();

                        // 타임바 기본설정
                        BaseTimeBarScript();

                        // 애니메이션 재생
                        UseAnimation();
                    }

                }

            }

        }

    }




}
