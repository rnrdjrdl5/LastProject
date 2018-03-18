using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTable : MonoBehaviour {

    public GameObject[] SubObjects;

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
