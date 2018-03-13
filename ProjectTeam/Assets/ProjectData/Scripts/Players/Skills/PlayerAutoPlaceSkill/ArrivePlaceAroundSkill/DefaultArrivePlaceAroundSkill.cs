using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용 설명 :  특정 부분까지 간 뒤에 충돌판정이나 스킬이 사용되야 한다면, 이 변수를 사용합니다.
// 애니메이션은 2단계로 나뉘어져있습니다.

// 반드시! 첫번째의 애니메이션 이름을 제대로 입력 해 주세요!

public class DefaultArrivePlaceAroundSkill : DefaultPlayerAutoPlaceSkill
{

    [Header(" - 목표지점 속도 설정")]
    [Tooltip(" - 목표지점까지 이동속도.")]
    public float PlayerMoveSpeed;


    // 스킬 입력으로 인한 이동여부. 
    private bool isMoveStart = false;

    private float StopDistance = 0.5f;

    public GameObject TestGameObject;
   



    // 1. 업데이트에서는 키인식을 받고, Fixed업데이트에서는 Rigidbody로 인한 이동제어를 받습니다.
    protected override void Update()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (CheckSkillKey())
            {
                if (CheckState())
                {
                    if (UseMouseSkill())
                    {
                        PlaceVector = FindLocationScript.GetPointToLocation(gameObject, PlayerCamera, MaxTargetDistance);


                        photonView.RPC("RPCStartFirstAnimation", PhotonTargets.All);
                        isMoveStart = true;

                    }

                }
            }
            
        }
    }

    protected void FixedUpdate()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (isMoveStart)
            {
                gameObject.GetComponent<Rigidbody>().velocity = (PlaceVector - transform.position).normalized * PlayerMoveSpeed;

                if ( (PlaceVector - transform.position).magnitude <= gameObject.GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime)
                {

                    Debug.Log("******거리안내******");
                    Debug.Log(transform.position);
                    Debug.Log(PlaceVector);
                    Debug.Log((transform.position - PlaceVector).sqrMagnitude);
                    Debug.Log(PlaceVector.normalized.magnitude);
                    Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude * Time.fixedDeltaTime);

                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    isMoveStart = false;
                    UseSkill();

                }
                else
                {
                    Debug.Log(PlaceVector);
                }
            }
        }
    }

    /************* 아래부터는 RPC 입니다. **************/

    // 주의! RPC는 상속이 되지 않습니다. 자식 스크립트에서 아래의 함수 명으로 된 RPC를 직접 재구현합시다.
    /*[PunRPC]
    void RPCStartFirstAnimation()
    {
        gameObject.GetComponent<Animator>().SetTrigger(FirstAnimationName);
    }*/
}
