using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePushObject : DefaultPushObject
{


    public override void Action()
    {
        if (GetInteractionObject().tag == "Interaction")
        {
            PhysicsTable pt = GetInteractionObject().gameObject.GetComponent<PhysicsTable>();

            if (pt != null)
            {

                Vector3 PlayerPosition = GetPlayerObject().transform.position;

                Vector3 v3 = (PlayerPosition - GetOriginalCameraPosition()).normalized;


                v3.y = YPower;


                if (pt.GetisCheck() == false)
                {

                    pt.SetisCheck(true);
                    pt.UnLockObjects();

                    GetInteractionObject().gameObject.GetComponent<Rigidbody>().AddForce(v3, ForceMode.Impulse);

                    GetInteractionObject().gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                    Quaternion QuatY = Quaternion.Euler(0.0f, 90.0f, 0.0f);



                    v3.y = 0;
                    v3 = QuatY * v3;

                    GetInteractionObject().gameObject.GetComponent<Rigidbody>().AddTorque(v3 * TorquePower);

                    if (GetPlayerObject().GetPhotonView().isMine)
                    {

                        PhotonNetwork.player.AddScore(PhotonNetwork.player.GetScore() + 1);
                    }


                }
            }
        }
    }
}
