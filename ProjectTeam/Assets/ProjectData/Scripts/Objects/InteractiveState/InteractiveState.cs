using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveState : MonoBehaviour {

    [Header(" - 상호작용 시전시간")]
    [Tooltip(" - 상호작용이 완료되기 까지의 시간입니다.(게이지에 사용)")]
    public float InteractiveTime = 0.0f;


    // 사용자가 이미 한번 뒤집었는지 판단하는 용도.
    private bool CanUseObject;

    public bool GetCanUseObject()
    {
        return CanUseObject;
    }

    public void SetCanUseObject(bool s)
    {
        CanUseObject = s;
    }

    // Use this for initialization

    private void Awake()
    {
        CanUseObject = true;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
