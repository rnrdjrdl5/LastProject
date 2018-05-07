using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionTorqueMove
{
    public void UseTorque()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = TorqueRad;
        gameObject.GetComponent<Rigidbody>().AddTorque(transform.up * TorqueRad, ForceMode.Impulse);
    }

    public void ResetObject()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = 0;

        SetCollisionMoveDirect(Vector3.zero);
        SetCollisionMoveSpeed(0);
        SetTorqueRad(0);
    }

}