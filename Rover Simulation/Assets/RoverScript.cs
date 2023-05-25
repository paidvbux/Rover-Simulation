using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverScript : MonoBehaviour
{
    public HingeJoint[] wheels;
    public float maxSpeed;
    float currForwardSpeed;
    float currBackwardSpeed;

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float wheelPowerLF = -input.y - input.x;
        float wheelPowerLR = -input.y - input.x;
        float wheelPowerRF = -input.y + input.x;
        float wheelPowerRR = -input.y + input.x;

        float max = Mathf.Max(new float[4] { Mathf.Abs(wheelPowerLF), Mathf.Abs(wheelPowerLR), Mathf.Abs(wheelPowerRF), Mathf.Abs(wheelPowerRR) });
        if (max > 1)
        {
            wheelPowerLF /= Mathf.Abs(max);
            wheelPowerLR /= Mathf.Abs(max);
            wheelPowerRF /= Mathf.Abs(max);
            wheelPowerRR /= Mathf.Abs(max);
        }

        if (input.x > 0 || -input.y > 0)
            currForwardSpeed = Mathf.Lerp(currForwardSpeed, maxSpeed, 0.1f);
        else
            currForwardSpeed = Mathf.Lerp(currForwardSpeed, 0, 0.1f);
        
        if (input.x < 0 || -input.y < 0)
            currBackwardSpeed = Mathf.Lerp(currBackwardSpeed, maxSpeed, 0.1f);
        else
            currBackwardSpeed = Mathf.Lerp(currBackwardSpeed, 0, 0.1f);

        wheelPowerLF *= wheelPowerLF > 0 ? currForwardSpeed : currBackwardSpeed;
        wheelPowerLR *= wheelPowerLR > 0 ? currForwardSpeed : currBackwardSpeed;
        wheelPowerRF *= wheelPowerRF > 0 ? currForwardSpeed : currBackwardSpeed;
        wheelPowerRR *= wheelPowerRR > 0 ? currForwardSpeed : currBackwardSpeed;

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
