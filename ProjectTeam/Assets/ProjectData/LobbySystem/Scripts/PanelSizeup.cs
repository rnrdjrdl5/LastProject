using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSizeup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C))
        {
            gameObject.GetComponent<RectTransform>().sizeDelta += Vector2.up * 10;
        }
	}
}
