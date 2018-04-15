using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CollisionTorqueMove
{
    protected override void Start()
    {
        base.Start();
        gameObject.GetComponent<Rigidbody>().maxAngularVelocity = TorqueRad;
        gameObject.GetComponent<Rigidbody>().AddTorque((transform.forward) * TorqueRad, ForceMode.Impulse);
    }


}