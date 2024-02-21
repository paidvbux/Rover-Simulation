using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationJointController : MonoBehaviour
{
    public float targetRotation;
    
    ArticulationBody articulation;

    void Start()
    {
        // 7 radians per second is the default and is too slow for this program.
        articulation = GetComponent<ArticulationBody>();
        articulation.maxAngularVelocity = 1000;
    }

    void FixedUpdate()
    {
        //In every fixed update call, rotate the joint to the "targetRotation" which is set by the
        // RobotController.cs attached to the robot arm base link
        RotateTo(targetRotation);
    }

    void RotateTo(float primaryAxisRotation)
    {
        ArticulationDrive drive = articulation.xDrive;
        drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }
}