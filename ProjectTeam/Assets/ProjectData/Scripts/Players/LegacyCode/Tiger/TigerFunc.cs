using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TigerScript{

   /* void PlayerBlinkFromMouse()     // 마우스 클릭으로 이동할 때.
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (pv.isMine)
            {
                if (isBlinkOn)
                {
                    MoveBlink();
                    pv.RPC("BlinkPosition", PhotonTargets.Others, transform.position);
                }
            }
        }
    }*/

    /*void MoveBlink()
    {
        transform.position = BlinkObject.transform.position;
        Destroy(BlinkObject);

        isBlinkOn = false;
    }*/

   /* void UseBlink()     // 스페이스바로 첫 시작.
    {
        if (pv.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isBlinkOn == false)
                {
                    CreateBlink();
                }
                else if (isBlinkOn == true)
                {
                    DeleteBlinkFunc();
                }
            }
        }
    }
    */
   /* void CreateBlink()
    {
        Vector3 BlinkObjectPosition = SetBlinkPosition();
        BlinkObject = Instantiate(BlinkPrefab, BlinkObjectPosition + Vector3.up * 0.5f, transform.rotation);

        isBlinkOn = true;
    }
    */
   /* private Vector3 SetBlinkPosition()
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
    }*/

   /* Vector3 PlayerMousePlaceRayCast()
    {
        Quaternion q2 = Quaternion.Euler(0,
            transform.eulerAngles.y, 0);

        Vector3 MouseVector3 = q2 * Vector3.forward;


        Quaternion q3 = Quaternion.Euler(PlayerCamera.GetComponent<PlayerCamera>().CameraRad, 0, 0);

        MouseVector3 = MouseVector3 * (q3 * Vector3.forward).z;
        MouseVector3.y = (q3 * Vector3.forward).y;

        return MouseVector3;
    }*/

    /*void DeleteBlinkFunc()
    {
        Destroy(BlinkObject);
        isBlinkOn = false;
    }*/

    /*void UpdateBlinkPosition()
    {
        if (pv.isMine)
        {
            if (isBlinkOn)
            {
                Vector3 BlinkObjectPosition = SetBlinkPosition();
                BlinkObject.transform.position = BlinkObjectPosition;
                BlinkObject.transform.rotation = transform.rotation;

            }
        }
    }*/

  /*  void UseWindBlast()
    {
        if(pv.isMine)
        {
            if(Input.GetMouseButtonDown(0))
            {

                if ( (!isBlinkOn) && (PlayerCondition != ConditionEnum.WINDBLAST) )
                {
                    PlayerAnimator.SetTrigger("WindBlastTrigger");
                    pv.RPC("WindBlastAnimation", PhotonTargets.Others);
                }
                    
            }

        }
    }
    */

   /* void CreateWindBlast()
    {
        CreateBullet();
    }

    void CreateBullet()
    {
        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = transform.forward * BulletDistance;
        BulletDefaultPlace.y += CharacterHeight;

        GameObject Bullet = Instantiate(BulletPrefab, transform.position + (BulletDefaultPlace), Quaternion.identity);



        Bullet.GetComponent<Bullet>().BulletDistance = transform.forward;
        Bullet.GetComponent<Bullet>().ShootPlayer = "Player" + pv.viewID;

        // 방향이 이상한 곳으로 튐.

    }
    */
    /*void Shooting(Vector3 Dis, string PlayerName)
    {
        float BulletDistance = 1.0f;
        float CharacterHeight = 1.2f;

        Vector3 BulletDefaultPlace = Dis * BulletDistance;
        BulletDefaultPlace.y += CharacterHeight;

        GameObject Bullet = Instantiate(BulletObject, transform.position + (BulletDefaultPlace), Quaternion.identity);
        Bullet.GetComponent<Bullet>().BulletDistance = Dis;
        Bullet.GetComponent<Bullet>().ShootPlayer = PlayerName;

    }
    */



}


