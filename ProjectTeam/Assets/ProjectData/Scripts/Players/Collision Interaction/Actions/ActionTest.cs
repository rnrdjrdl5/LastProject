using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : Photon.PunBehaviour {

    private GameObject PlayerCamera;
    private GameObject PlayerMainObject;

    private void Start()
    {
        PlayerCamera = GameObject.Find("PlayerCamera");
        PlayerMainObject = gameObject.GetComponentInParent<PlayerState>().gameObject;
    }


    bool isMove = false;
    bool isCheck = false;
	// Update is called once per frame

    public float XZPower = 0.0f;
    public float YPower = 0.0f;

    public float TorquePower = 0.0f;


    private void OnTriggerStay(Collider other)
    {

        


            if (other.tag == "Interaction")
            {


                PhysicsTable pt = other.gameObject.GetComponent<PhysicsTable>();

            if (pt != null)
            {
                Vector3 CameraPosition = PlayerCamera.transform.position;
                Vector3 PlayerPosition = PlayerMainObject.transform.position;

                Vector3 v3 = (PlayerPosition - CameraPosition).normalized;


                v3.y = YPower;

                Debug.Log(v3);
                if (gameObject.GetComponentInParent<PlayerState>().GetPlayerCondition() == PlayerState.ConditionEnum.WINDBLAST &&
                    pt.GetisCheck() == false)
                {
                    pt.SetisCheck(true);
                    pt.UnLockObjects();

                    other.gameObject.GetComponent<Rigidbody>().AddForce(v3, ForceMode.Impulse);


                    Quaternion QuatY = Quaternion.Euler(0.0f, 90.0f, 0.0f);



                    v3.y = 0;
                    v3 = QuatY * v3;

                    other.gameObject.GetComponent<Rigidbody>().AddTorque(v3 * TorquePower);

                    if (PlayerMainObject.GetPhotonView().isMine)
                    {
                        PhotonNetwork.player.AddScore(PhotonNetwork.player.GetScore() + 1);
                    }


                }
            }
        }
    }

}
