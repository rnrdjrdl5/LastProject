using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionTorqueMove
{
    public void UseTorque()
    {
        gameObject.GetComponent<Rigidbody>().AddTorque(-originalTorque,ForceMode.Impulse);

            originalTorque = (transform.forward) * TorqueRad;
            gameObject.GetComponent<Rigidbody>().maxAngularVelocity = TorqueRad;
            gameObject.GetComponent<Rigidbody>().AddTorque(originalTorque, ForceMode.Impulse);

    }


}