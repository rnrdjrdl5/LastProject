using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public List<GameObject> InterObj;

    public void AddInterObj(GameObject go) { InterObj.Add(go); }

    public GameObject FindObject(int vID)
    {
        for (int i = 0; i < InterObj.Count; i++)
        {


            if (InterObj[i].GetPhotonView().viewID == vID)
                return InterObj[i];
        }
        return null;
    }
}
