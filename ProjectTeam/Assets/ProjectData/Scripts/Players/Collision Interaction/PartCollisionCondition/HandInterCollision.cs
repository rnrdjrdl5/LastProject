﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInterCollision : DefaultPartCollisionCondition
{



    private void OnTriggerStay(Collider other)
    {

        // 충돌 대상이 상호작용 tag를 가졌다면

        // 이미 키값 자체는 hand 에 온 상태. other가 안가지고 있다.
        if (other.tag == "Interaction")
        {

            InteractiveState interactiveState = other.GetComponent<InteractiveState>();

            if (interactiveState.InterObjectType == InterObjectType)
            {
                Debug.Log("2");
                // 만약 애니메이션 기간 중 특정 구간에서 공격이 가능하고 &&
                // 공격이 한번도 사용되지 않았다면
                if (GetisCanInterAction() == true && isUseInterAction == false)
                {
                    interactiveState.SetCanUseObject(false);
                    // 공격을 시작합니다.
                    GetDefaultInteraction().StartInterAction(other.gameObject);

                    Debug.Log("1");



                    // 두번다시 공격 인식이 안되도록 설정합니다.
                    isUseInterAction = true;

                    // 이 오브젝트는 공격이 이미 됐다고 알립니다.
                    if(GetDefaultInteraction().photonView.isMine)
                    {
                        interactiveState.CallOffCanUseObject();
                    }

                }
            }
        }
    }
}
