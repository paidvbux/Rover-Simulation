using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RobotController : MonoBehaviour
{
    public ArticulationJointController[] joints;
    public void RotateJoint(int jointIndex, float targetRotation)
    {
        joints[jointIndex].targetRotation = targetRotation;
    }
}

