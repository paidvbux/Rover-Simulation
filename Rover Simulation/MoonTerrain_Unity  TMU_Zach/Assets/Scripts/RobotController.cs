using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotController : MonoBehaviour
{
    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;


    public void RotateJoint(int jointIndex, float targetRotation)
    {   // setting the targetRotation value over the "ArticulationJointController" object for each join    
        Joint joint = joints[jointIndex];
        UpdateRotationState(targetRotation, joint.robotPart);
    }
    
    private void UpdateRotationState(float targetRotation, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.targetRotation = targetRotation;
    }
   

}
