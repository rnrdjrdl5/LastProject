using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    bool isMove = false;
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q)) {
            isMove = true;
                }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            isMove = false;
        }
        if(isMove == true)
        {
            transform.position += Vector3.up * 2 * Time.deltaTime;
        }



	}

    public float XZPower = 0.0f;
    public float YPower = 0.0f;

    public float TorquePower = 0.0f; 

    private void OnTriggerStay(Collider other)
    {
        Vector3 v3 = (other.transform.position - transform.position).normalized * XZPower;
        v3.y = YPower;

        if (other.tag == "Table")
        {
            Debug.Log("충돌");
            other.gameObject.GetComponent<Rigidbody>().AddForce(v3 , ForceMode.Impulse);


            Quaternion QuatY = Quaternion.Euler(0.0f,90.0f, 0.0f);

            

            v3.y = 0;
            v3 = QuatY * v3;

            other.gameObject.GetComponent<Rigidbody>().AddTorque(v3 * TorquePower);


        }
    }
}
