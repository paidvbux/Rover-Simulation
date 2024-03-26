using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationJointController : MonoBehaviour
{
  
    private ArticulationBody articulation;
    [HideInInspector]
    public float targetRotation;
     

    public Vector3 AxisVector;

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();

        articulation.maxAngularVelocity = 1000;
    }

    void FixedUpdate() 
    {
        RotateTo(targetRotation);
        //RotateByTorque(targetRotation);
       

     }


    // MOVEMENT HELPERS


    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;

        //Debug.Log(primaryAxisRotation);

    }

    void RotateByTorque(float jointTorque)
    {
        
        var drive = articulation.xDrive;
        drive.stiffness =0;
        drive.damping = 100;

        articulation.xDrive = drive; 
        
        articulation.AddRelativeTorque(AxisVector* jointTorque);
        //articulation.AddTorque(AxisVector* jointTorque);

        //Debug.Log(jointTorque);
    }

}
