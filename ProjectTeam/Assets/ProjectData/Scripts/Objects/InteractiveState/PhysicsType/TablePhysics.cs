using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************************
 * 
 *  작성자  : 반재억
 *  작성일자 : 2018. 03.30
 *  사용이유 : 테이블 상호작용 물리적용
 *  
 *  고려사항 : MonoBehaviour 상속받지말고
 *             InteractiveState 의 클래스로써 선언하는것도 방법이다.
 *             >  SubObject를 받아올 방법이 필요하게됨.
 *  *****************************************/


public class TablePhysics : MonoBehaviour {

    /**** public ****/
    public              float               YPower;             // 위로 솟는 힘
    public              float               XZPower;            // 직선으로 솟는 힘
    public              float               TorquePower;        // 회전하는 힘

    public float tableMass;

    public float subObjectMass;

    public GameObject[] subObjects;                // 물리의 영향을 받는 보조 오브젝트 (식기, 접시 등)


    /**** private ****/
    private Rigidbody rigidBody;

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = tableMass;
    }



    public void Action(Vector3 NormalVector3)
    {

        // 물체가 날라갈 방향, XZ 축
        Vector3 NotY_NormalVector3 = NormalVector3;
        NotY_NormalVector3.y = 0;


        // 모든 회전과 이동 가능
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.maxAngularVelocity = TorquePower;

        // 모든 보조 오브젝트 영향 받도록 설정
        UnLockSubObjects();

        // XZ축 힘 
        rigidBody.AddForce( NotY_NormalVector3 * XZPower, ForceMode.Impulse);

        //// Y축 힘
        rigidBody.AddForce(transform.up * YPower, ForceMode.Impulse);

        // 이동방향으로 회전방향 구하기
        Quaternion QuatY = Quaternion.Euler(0.0f, 90.0f, 0.0f);

        // 회전시키기
        rigidBody.AddTorque((QuatY * NotY_NormalVector3) * TorquePower, ForceMode.Impulse);
        
    }

    // 보조 오브젝트 연결 해제
    private void UnLockSubObjects()
    {
        // for문
        for (int i = subObjects.Length - 1; i >= 0; i--)
        {

            // 보조 오브젝트의 부모 없음으로 처리
            subObjects[i].transform.parent = null;

            // 강체 받아오기
            Rigidbody subRigidbody = subObjects[i].GetComponent<Rigidbody>();

            // 보조 오브젝트 물리 설정
            subRigidbody.constraints = RigidbodyConstraints.None;
            subRigidbody.maxAngularVelocity = TorquePower;
            subRigidbody.mass = subObjectMass;
        }
    }
}
