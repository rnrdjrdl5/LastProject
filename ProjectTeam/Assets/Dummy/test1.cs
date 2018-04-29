using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour {

    public Material m;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.7f, 0.7f, 1.0f, 0.5f);
        }
	}
}
