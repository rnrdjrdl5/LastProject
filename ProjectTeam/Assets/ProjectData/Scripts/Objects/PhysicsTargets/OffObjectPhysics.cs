using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌 시 N초 뒤에 물리 제거
public class OffObjectPhysics : MonoBehaviour {

    public float OffTime;
    IEnumerator CoroOffPhysics;
    bool isCheck;

    public GameObject OtherObject;                // 달라붙어있는 오브젝트 있으면 선택

    private void Awake()
    {
        CoroOffPhysics = OffPhysics();
        isCheck = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor") && !isCheck)
        {
            isCheck = true;
            StartCoroutine(CoroOffPhysics);
        }
    }

    IEnumerator OffPhysics()
    {
        yield return new WaitForSeconds(OffTime);

        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<BoxCollider>());

        if (OtherObject != null)
        {
            Destroy(OtherObject.GetComponent<Rigidbody>());
            Destroy(OtherObject.GetComponent<BoxCollider>());
        }

        yield return null;
    }
}
