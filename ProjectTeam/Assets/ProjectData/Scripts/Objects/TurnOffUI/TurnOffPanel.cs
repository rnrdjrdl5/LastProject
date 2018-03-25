using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffPanel : MonoBehaviour {

    private float TurnOffTime = 0;

    public float GetTurnOffTime() { return TurnOffTime; }
    public void SetTurnOffTime(float tot) { TurnOffTime = tot; }

    private void Update()
    {
        Debug.Log("aaa");
        TurnOffTime -= Time.deltaTime;
        if(TurnOffTime<=0)
        {
            Destroy(gameObject);
        }
    }
}
