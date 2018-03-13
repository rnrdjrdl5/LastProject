using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************
*
* 코더 : 반재억
* 일자 : 2018-02-21
* 코딩목적 : 마우스로 특정 지점을 편하게 지정할 수 있도록 사용.
* 특이사항 
*  - 해당 기능은 Blink 내부의 함수와 같습니다.
*    Blink 내부의 함수를 이 스크립트의 함수를 사용하도록 교체합시다. 
*    
**********************************************/
public class PointToLocation{

    // 해당 변수는 레이캐스트의 무한 거리를 의미합니다.
    // 마우스 위치로 지정한 거리만큼 레이캐스트를 쐈을 때,
    // 도착하지 않는다면 끝지점부터 수직으로 레이캐스트를 쏩니다.
    // ex) 사이퍼즈의 고각 스킬.
    private float           RaycastInfinity             = 10000.0f;




    public Vector3 GetPointToLocation(GameObject UseObject , GameObject PlayerCamera , float MaxLocationDistance)
    {
        RaycastHit hit;

        Vector3 MouseVector3 = FindMouseCursorPosition(UseObject, PlayerCamera);

        
        Vector3 BlinkVector3 = Vector3.zero;
        if (Physics.Raycast(PlayerCamera.GetComponent<Transform>().position, MouseVector3, out hit, MaxLocationDistance, 1 << LayerMask.NameToLayer("Floor")))
        {
            BlinkVector3 = hit.point;
        }

        else
        {
            // 1. 방향을 거리적용
            Vector3 NotTargetBlink = PlayerCamera.GetComponent<Transform>().position + MouseVector3 * MaxLocationDistance;

            if (Physics.Raycast(NotTargetBlink, Vector3.down, out hit, RaycastInfinity, 1 << LayerMask.NameToLayer("Floor")))
            {
 
                BlinkVector3 = hit.point;
                //   BlinkVector3 += Vector3.up * 0.5f;
            }
            else
            {
                Debug.Log("레이캐스트 못찾았다. 위치를  본인위치로 돌려준다.");
                return UseObject.transform.position;

            }
        }

        return BlinkVector3;
      
    }


    
    private Vector3 FindMouseCursorPosition(GameObject UseObject , GameObject PlayerCamera)
    {
        Quaternion q2 = Quaternion.Euler(0,
            UseObject.transform.eulerAngles.y, 0);

        Vector3 MouseVector3 = q2 * Vector3.forward;


        Quaternion q3 = Quaternion.Euler(PlayerCamera.GetComponent<PlayerCamera>().CameraRad, 0, 0);

        MouseVector3 = MouseVector3 * (q3 * Vector3.forward).z;
        MouseVector3.y = (q3 * Vector3.forward).y;


        return MouseVector3;
    }

   /* private Transform UpdateBlinkPosition()
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

        
    }*/
}
