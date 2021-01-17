using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPlatformPhysics : MonoBehaviour
{
    private Plane plane;
    void Start() {
        //Setup the consteaints of the pivoting platform as editor was not allowing changes
        plane = new Plane(Vector3.back, transform.position);

        HingeJoint hinge = GetComponent<HingeJoint>();

        JointLimits limits = hinge.limits;
        limits.min = -20;
        limits.bounciness = 0;
        limits.bounceMinVelocity = 0;
        limits.max = 20;
        hinge.limits = limits;
        hinge.useLimits = true;
    }
}
