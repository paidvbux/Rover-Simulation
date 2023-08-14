using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RobotController : MonoBehaviour
{
    public ArticulationJointController[] joints;
    public void RotateJoint(int jointIndex, float targetRotation)
    {
        UpdateRotationState(targetRotation, joints[jointIndex]);
    }

    void UpdateRotationState(float targetRotation, ArticulationJointController robotPart)
    {
        robotPart.targetRotation = targetRotation;
    }
}

