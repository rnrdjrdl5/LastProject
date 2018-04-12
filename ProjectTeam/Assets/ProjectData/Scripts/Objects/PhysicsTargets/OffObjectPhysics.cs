using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌 시 N초 뒤에 물리 제거
public class OffObjectPhysics : MonoBehaviour {

    public float OffTime;
    IEnumerator CoroOffPhysics;
    bool isCheck;

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

        BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();


        for (int i = 0; i < boxColliders.Length; i++)
        {
            Destroy(boxColliders[i]);
        }

        Destroy(GetComponent<Rigidbody>());


        yield return null;
    }
}
