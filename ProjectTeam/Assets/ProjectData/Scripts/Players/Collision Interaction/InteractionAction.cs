using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAction : MonoBehaviour {


    public float XZPower = 0.0f;
    public float YPower = 0.0f;

    public float TorquePower = 0.0f;


    private GameObject PlayerCamera;



    private GameObject InteractionObject;

    public GameObject GetInteractionObject() { return InteractionObject; }
    public void SetInteractionObject(GameObject GO) { InteractionObject = GO; }


    private void Start()
    {
        PlayerCamera = GameObject.Find("PlayerCamera");
    }

    public void Action()
    {
        if (InteractionObject.tag == "Interaction")
        {


            PhysicsTable pt = InteractionObject.gameObject.GetComponent<PhysicsTable>();

            if (pt != null)
            {
                Vector3 CameraPosition = PlayerCamera.transform.position;
                Vector3 PlayerPosition = gameObject.transform.position;

                Vector3 v3 = (PlayerPosition - CameraPosition).normalized;


                v3.y = YPower;

                
                if(pt.GetisCheck() == false)
                {
                    pt.SetisCheck(true);
                    pt.UnLockObjects();

                    InteractionObject.gameObject.GetComponent<Rigidbody>().AddForce(v3, ForceMode.Impulse);


                    Quaternion QuatY = Quaternion.Euler(0.0f, 90.0f, 0.0f);



                    v3.y = 0;
                    v3 = QuatY * v3;

                    InteractionObject.gameObject.GetComponent<Rigidbody>().AddTorque(v3 * TorquePower);

                    if (gameObject.GetPhotonView().isMine)
                    {
                        PhotonNetwork.player.AddScore(PhotonNetwork.player.GetScore() + 1);
                    }


                }
            }
        }
    }
}
