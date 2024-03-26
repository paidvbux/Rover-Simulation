using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWheelScript : MonoBehaviour
{

    ArticulationBody articulation;
    float wheelCounter;

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    void FixedUpdate()
    {
        //RotateWheel(500);
        //articulation.AddTorque(Vector3.forward * 300);
        //articulation.AddRelativeTorque(Vector3.forward * 300);
    }

    // MOVEMENT HELPERS

    public void RotateWheel(float angularSpeed)
    {
        float rotationChange = angularSpeed * Time.fixedDeltaTime;
        float rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
        RotateTo(rotationGoal);
    }

    float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = articulation.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }
}
