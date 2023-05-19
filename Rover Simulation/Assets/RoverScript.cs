using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverScript : MonoBehaviour
{
    public HingeJoint[] wheels;

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float wheelPowerLF = input.y - input.x;
        float wheelPowerLR = input.y - input.x;
        float wheelPowerRF = input.y + input.x;
        float wheelPowerRR = input.y + input.x;

        float max = Mathf.Max(new float[4] { Mathf.Abs(wheelPowerLF), Mathf.Abs(wheelPowerLR), Mathf.Abs(wheelPowerRF), Mathf.Abs(wheelPowerRR) });
        if (max > 1)
        {
            wheelPowerLF /= Mathf.Abs(max);
            wheelPowerLR /= Mathf.Abs(max);
            wheelPowerRF /= Mathf.Abs(max);
            wheelPowerRR /= Mathf.Abs(max);
        }

        wheelPowerLF *= 500;
        wheelPowerLR *= 500;
        wheelPowerRF *= 500;
        wheelPowerRR *= 500;

        wheels[0].motor = ChangeTargetVelocity(wheels[0], wheelPowerLF);
        wheels[1].motor = ChangeTargetVelocity(wheels[1], wheelPowerLR);
        wheels[2].motor = ChangeTargetVelocity(wheels[2], wheelPowerRF);
        wheels[3].motor = ChangeTargetVelocity(wheels[3], wheelPowerRR);       
    }

    JointMotor ChangeTargetVelocity(HingeJoint joint, float input)
    {
        JointMotor target = joint.motor;
        target.targetVelocity = input;
        return target;
    } 
}
