using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindUsedObject : MonoBehaviour {

    // Use this for initialization
    private void Awake()
    {

    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        /*
        // 충돌물체가 상호물체인 경우
        if(other.tag == "Interaction")
        {
            if (other.gameObject.GetComponent<InteractiveState>().GetCanUseObject() == false)
            {
            }
        }*/
    }
}
