using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTable : MonoBehaviour {

    public GameObject[] SubObjects;


    // 해당 isCheck : 
     // 충돌체크 시 여러번의 충돌체크가 일어나지 않도록 막습니다.
     // 단, 물리충돌만 막아버립니다.


    private bool isCheck = false;
    public bool GetisCheck() { return isCheck; }
    public void SetisCheck(bool IC) { isCheck = IC; }

    
    public float SubObjectMass = 1.0f;

    public void UnLockObjects()
    {
        for(int i = 0; i < SubObjects.Length; i++)
        {
            SubObjects[i].transform.parent = null;
            Rigidbody rd = SubObjects[i].AddComponent<Rigidbody>();
            rd.mass = SubObjectMass;
        }
    }

}
