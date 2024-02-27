using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArticulationBody))]
public class MotorScript : MonoBehaviour
{
    [SerializeField] ArticulationBody body => GetComponent<ArticulationBody>();
    public float target;

    public void rotateTo(float angle, bool additive)
    {
        target += angle;
        ArticulationDrive drive = body.xDrive;
        drive.target = target;
        body.xDrive = drive;
    }

    public void rotateTo(float angle)
    {
        target = angle;
        ArticulationDrive drive = body.xDrive;
        drive.target = target;
        body.xDrive = drive;
    }
}
