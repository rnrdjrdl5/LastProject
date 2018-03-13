using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Blink
{

    void SetPlayerCamera()
    {
        PlayerCamera = GameObject.Find("PlayerCamera");
    }

    void UseBlink()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if((Input.GetKeyDown(KeyCode.Space)) && 
                (gameObject.GetComponent<PlayerState>().GetPlayerCondition() == PlayerState.ConditionEnum.IDLE ||
                gameObject.GetComponent<PlayerState>().GetPlayerCondition() == PlayerState.ConditionEnum.RUN) )    
            {

                if ( (isBlinkOn == false) &&
                    (gameObject.GetComponent<PlayerState>().GetisUseSkillMode() == false) )
                {
                    CreateBlink();

                    gameObject.GetComponent<PlayerState>().SetisUseSkillMode(true);
                }
                else if (isBlinkOn == true)
                {
                    DeleteBlinkFunc();
                    gameObject.GetComponent<PlayerState>().SetisUseSkillMode(false);

                }
            }
        }
    }

    void CreateBlink()
    {
        Vector3 BlinkObjectPosition = SetBlinkPosition();
        BlinkObject = Instantiate(BlinkPrefab, BlinkObjectPosition + Vector3.up * 0.5f, transform.rotation);

        isBlinkOn = true;
    }

    void DeleteBlinkFunc()
    {
        Destroy(BlinkObject);
        isBlinkOn = false;
    }

    private Vector3 SetBlinkPosition()
    {
        RaycastHit hit;

        Vector3 MouseVector3 = PlayerMousePlaceRayCast();


        Vector3 BlinkVector3 = Vector3.zero;
        if (Physics.Raycast(PlayerCamera.GetComponent<Transform>().position, MouseVector3, out hit, BlinkDistance, 1 << LayerMask.NameToLayer("Floor")))
        {
            BlinkVector3 = hit.point;
        }

        else
        {
            // 1. 방향을 거리적용
            Vector3 NotTargetBlink = PlayerCamera.GetComponent<Transform>().position + MouseVector3 * BlinkDistance;

            if (Physics.Raycast(NotTargetBlink, Vector3.down, out hit, RaycastInfinity, 1 << LayerMask.NameToLayer("Floor")))
            {
                BlinkVector3 = hit.point;
                //   BlinkVector3 += Vector3.up * 0.5f;
            }
            else
            {
                Debug.Log("레이캐스트 못찾았다. 위치를  본인위치로 돌려준다.");
                return transform.position;

            }
        }

        return BlinkVector3;
    }

    Vector3 PlayerMousePlaceRayCast()
    {
        Quaternion q2 = Quaternion.Euler(0,
            transform.eulerAngles.y, 0);

        Vector3 MouseVector3 = q2 * Vector3.forward;


        Quaternion q3 = Quaternion.Euler(PlayerCamera.GetComponent<PlayerCamera>().CameraRad, 0, 0);

        MouseVector3 = MouseVector3 * (q3 * Vector3.forward).z;
        MouseVector3.y = (q3 * Vector3.forward).y;

        return MouseVector3;
    }

    private void UpdateBlinkPosition()
    {
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            if (isBlinkOn)
            {
                Vector3 BlinkObjectPosition = SetBlinkPosition();
                BlinkObject.transform.position = BlinkObjectPosition;
                BlinkObject.transform.rotation = transform.rotation;

            }
        }
    }

    void MoveBlink()
    {
        transform.position = BlinkObject.transform.position;
        Destroy(BlinkObject);

        isBlinkOn = false;
    }

    void PlayerBlinkFromMouse()     // 마우스 클릭으로 이동할 때.
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameObject.GetComponent<PhotonView>().isMine)
            {
                if (isBlinkOn)
                {
                    MoveBlink();
                    gameObject.GetComponent<PhotonView>().RPC("BlinkPosition", PhotonTargets.Others, transform.position);
                    gameObject.GetComponent<PlayerState>().SetisUseSkillMode(false);
                }
            }
        }
    }



    /******** 여기서부터는 RPC 함수 입니다. ********/
    /*
    [PunRPC]
    void BlinkPosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        // 시리얼라이즈에서 이미 받고 나서 
        // update 로 가기 전에
        // RPC가 들어오면,
        // RPC로 인한 위치이동 > update로 인한 보간  이 순서대로 적용되서
        // RPC로 위치 이동했다가 보간위치로 이동함.
        // 그래서 시리얼라이즈로 받는 값도 바꿔준다.
        //     RecvPosition = newPosition;

        // 도착 후 생성? Instantiate(BlinkEffect, transform.position, Quaternion.identity);
    }
    */
}
