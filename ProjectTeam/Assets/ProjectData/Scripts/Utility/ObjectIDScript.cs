using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIDScript : MonoBehaviour {

    public static int MaxID = 0;

    public int ID;

    // Use this for initialization
    private void Awake()
    {

    }

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetID()
    {
        
        ID = MaxID;
        IncreaseMaxID();
    }

    public void DeleteID()
    {
        // 빈자리를 채우지 않는다.
        ID = -1;
    }

    public void IncreaseMaxID()
    {
        if (MaxID > 1000)
        {
            Debug.Log("asdf");
            MaxID = 0;
        }
        else
            MaxID++;
    }
}
