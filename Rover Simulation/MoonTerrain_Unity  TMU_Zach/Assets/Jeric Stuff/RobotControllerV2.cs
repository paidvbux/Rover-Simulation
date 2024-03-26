using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControllerV2 : MonoBehaviour
{
    public GameObject[] joints;

    public void RotateJoint(int jointIndex, float targetRotation)
    {   // setting the targetRotation value over the "ArticulationJointController" object for each join    
        GameObject joint = joints[jointIndex];
        UpdateRotationState(targetRotation, joint);
    }

    private void UpdateRotationState(float targetRotation, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.targetRotation = targetRotation;
    }
}
