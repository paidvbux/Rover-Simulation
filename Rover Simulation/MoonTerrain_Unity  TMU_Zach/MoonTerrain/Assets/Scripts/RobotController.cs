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


    // CONTROL

   

    public void RotateJoint(int jointIndex, float targetRotation)
    {
       
        Joint joint = joints[jointIndex];
        UpdateRotationState(targetRotation, joint.robotPart);
    }
    

    // HELPERS

    static void UpdateRotationState(float targetRotation, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.targetRotation = targetRotation;
    }
   

}
