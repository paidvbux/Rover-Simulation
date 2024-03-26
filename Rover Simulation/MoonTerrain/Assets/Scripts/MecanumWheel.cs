using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider FowaredHeadingWheel;
    public WheelCollider SidewayRollingWheel;
    public bool motor;
    public bool steering;
}

public class MecanumWheel : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringTorque;
    [HideInInspector]
    public int typeOfOperation; //2: Chassis follow Global X,Y, Turn tragectory command with physics interaction between wheels to surface;
                                //1: Chassis follow Local Torque command with physics interaction between wheels to surface;
                                //0: tele-operation
    [HideInInspector]
    public float Vx;
    [HideInInspector]
    public float Vy;
    [HideInInspector]
    public float Vturn;

    private Vector3 pos_last;
    public Rigidbody robotBody;
    private float Angle_last;
    [HideInInspector]
    public float globalAngle;

    private float err_x_last=0;
    private float err_y_last =0;
    private float err_turn_last=0;


    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider, double RPM)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        //visualWheel.transform.rotation = rotation;
        visualWheel.transform.Rotate(0.05f* (float)RPM, 0, 0, Space.Self);
    }

    private void Start()
    {
        Angle_last = -robotBody.transform.rotation.eulerAngles.y;
        globalAngle = Angle_last;

        maxMotorTorque = maxMotorTorque * (0.5f -Physics.gravity.y / 20f);
        maxSteeringTorque = maxSteeringTorque * (0.5f - Physics.gravity.y / 20f);
    }

    public void FixedUpdate()
    {
        if (typeOfOperation == 1)
            ChassisMotionwithPhysics();
        else if (typeOfOperation == 2)
            ChassisMotionGlobalXYPhysics();
        else ChassisMotionTeleOp();

        //ApplyLocalPositionToVisuals(axleInfo.leftWheel);
        //ApplyLocalPositionToVisuals(axleInfo.rightWheel);
    }

    private void ChassisMotionwithPhysics()
    {
    float motor     =   Vx * maxMotorTorque  ;
    float strafe    =   Vy* maxMotorTorque ;
    float turn      =   Vturn* maxSteeringTorque ;

    float LF = motor - strafe - turn;
    float RF = motor + strafe + turn;
    float LR = motor + strafe - turn;
    float RR = motor - strafe + turn;

        //LF
        axleInfos[0].FowaredHeadingWheel.motorTorque = LF;
        ApplyLocalPositionToVisuals(axleInfos[0].FowaredHeadingWheel, axleInfos[0].FowaredHeadingWheel.rpm + axleInfos[0].SidewayRollingWheel.rpm);
        axleInfos[0].SidewayRollingWheel.motorTorque = LF;
        //RF
        axleInfos[1].FowaredHeadingWheel.motorTorque = RF;
        ApplyLocalPositionToVisuals(axleInfos[1].FowaredHeadingWheel, axleInfos[1].FowaredHeadingWheel.rpm - axleInfos[1].SidewayRollingWheel.rpm);
        axleInfos[1].SidewayRollingWheel.motorTorque = -RF;
        //LR
        axleInfos[2].FowaredHeadingWheel.motorTorque = LR;
        ApplyLocalPositionToVisuals(axleInfos[2].FowaredHeadingWheel, axleInfos[2].FowaredHeadingWheel.rpm - axleInfos[2].SidewayRollingWheel.rpm);
        axleInfos[2].SidewayRollingWheel.motorTorque = -LR;
        //RR
        axleInfos[3].FowaredHeadingWheel.motorTorque = RR;
        ApplyLocalPositionToVisuals(axleInfos[3].FowaredHeadingWheel, axleInfos[3].FowaredHeadingWheel.rpm + axleInfos[3].SidewayRollingWheel.rpm);
        axleInfos[3].SidewayRollingWheel.motorTorque = RR;
    }

    private void ChassisMotionTeleOp()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float strafe = -maxMotorTorque * Input.GetAxis("Horizontal");
        float turn = maxSteeringTorque * Input.GetAxis("Turn");
        

        float LF = motor - strafe - turn;
        float RF = motor + strafe + turn;
        float LR = motor + strafe - turn;
        float RR = motor - strafe + turn;

        //Debug.Log("X" + robotBody.transform.position.z);
        //Debug.Log("Y" + robotBody.transform.position.x);
        //Debug.Log("Turn" + robotBody.transform.rotation.eulerAngles.y );

        //LF
        axleInfos[0].FowaredHeadingWheel.motorTorque = LF;
        ApplyLocalPositionToVisuals(axleInfos[0].FowaredHeadingWheel, axleInfos[0].FowaredHeadingWheel.rpm + axleInfos[0].SidewayRollingWheel.rpm);
        axleInfos[0].SidewayRollingWheel.motorTorque = LF;
        //RF
        axleInfos[1].FowaredHeadingWheel.motorTorque = RF;
        ApplyLocalPositionToVisuals(axleInfos[1].FowaredHeadingWheel, axleInfos[1].FowaredHeadingWheel.rpm - axleInfos[1].SidewayRollingWheel.rpm);
        axleInfos[1].SidewayRollingWheel.motorTorque = -RF;
        //LR
        axleInfos[2].FowaredHeadingWheel.motorTorque = LR;
        ApplyLocalPositionToVisuals(axleInfos[2].FowaredHeadingWheel, axleInfos[2].FowaredHeadingWheel.rpm - axleInfos[2].SidewayRollingWheel.rpm);
        axleInfos[2].SidewayRollingWheel.motorTorque = -LR;
        //RR
        axleInfos[3].FowaredHeadingWheel.motorTorque = RR;
        ApplyLocalPositionToVisuals(axleInfos[3].FowaredHeadingWheel, axleInfos[3].FowaredHeadingWheel.rpm + axleInfos[3].SidewayRollingWheel.rpm);
        axleInfos[3].SidewayRollingWheel.motorTorque = RR;
    }

    private void ChassisMotionGlobalXYPhysics()
    {

        float err_x = Vx - robotBody.transform.position.z;
        float err_y = Vy + robotBody.transform.position.x;
        float err_turn = Vturn - getAngle();

        Debug.Log("X" + robotBody.transform.position.z);
        Debug.Log("Y" + robotBody.transform.position.x);
        Debug.Log("Angle" + getAngle());

        float cosTheta = Mathf.Cos(getAngle() * Mathf.Deg2Rad);
        float sinTheta = Mathf.Sin(getAngle() * Mathf.Deg2Rad);

        float V_x = 5 * err_x + 50 * (err_x - err_x_last);
        float V_y = 5 * err_y + 50 * (err_y - err_y_last);
        err_x_last = err_x;
        err_y_last = err_y;

        //mapping from gloabl to local robot frame


        float motor = (V_x*cosTheta + V_y *sinTheta)* maxMotorTorque ;
        float strafe = (-V_x * sinTheta + V_y * cosTheta) * maxMotorTorque ;

        float maxForce = Mathf.Max(Mathf.Abs(motor), Mathf.Abs(strafe));
        if (maxForce > maxMotorTorque)
        {
            motor = motor * maxMotorTorque / maxForce;
            strafe = strafe * maxMotorTorque / maxForce;
        }
        float turn = (err_turn + 15*(err_turn- err_turn_last )) * maxSteeringTorque;
        err_turn_last = err_turn;

        if (Mathf.Abs(turn) > maxSteeringTorque)
            turn = maxSteeringTorque * Mathf.Sign(turn);
        Debug.Log("PWD" + turn);

        float LF = motor - strafe - turn;
        float RF = motor + strafe + turn;
        float LR = motor + strafe - turn;
        float RR = motor - strafe + turn;
        
        //LF
        axleInfos[0].FowaredHeadingWheel.motorTorque = LF;
        ApplyLocalPositionToVisuals(axleInfos[0].FowaredHeadingWheel, axleInfos[0].FowaredHeadingWheel.rpm + axleInfos[0].SidewayRollingWheel.rpm);
        axleInfos[0].SidewayRollingWheel.motorTorque = LF;
        //RF
        axleInfos[1].FowaredHeadingWheel.motorTorque = RF;
        ApplyLocalPositionToVisuals(axleInfos[1].FowaredHeadingWheel, axleInfos[1].FowaredHeadingWheel.rpm - axleInfos[1].SidewayRollingWheel.rpm);
        axleInfos[1].SidewayRollingWheel.motorTorque = -RF;
        //LR
        axleInfos[2].FowaredHeadingWheel.motorTorque = LR;
        ApplyLocalPositionToVisuals(axleInfos[2].FowaredHeadingWheel, axleInfos[2].FowaredHeadingWheel.rpm - axleInfos[2].SidewayRollingWheel.rpm);
        axleInfos[2].SidewayRollingWheel.motorTorque = -LR;
        //RR
        axleInfos[3].FowaredHeadingWheel.motorTorque = RR;
        ApplyLocalPositionToVisuals(axleInfos[3].FowaredHeadingWheel, axleInfos[3].FowaredHeadingWheel.rpm + axleInfos[3].SidewayRollingWheel.rpm);
        axleInfos[3].SidewayRollingWheel.motorTorque = RR;
        
    }

    private float getAngle()
    {
        float angle = -robotBody.transform.rotation.eulerAngles.y;

        float Angle_change = angle - Angle_last;

        if (Angle_change > 180)
            Angle_change -= 360;
        else if (Angle_change < -180)
            Angle_change += 360;

        globalAngle += Angle_change;
        Angle_last = angle;
        return globalAngle;
    }

    public void surfaceFriction (float frictionCoefficience)
    {

        // to simulate the surface friction
        WheelFrictionCurve robotWheelFrictionCurve;

        for (int i =0; i<4;i ++)
        {
            robotWheelFrictionCurve = axleInfos[i].FowaredHeadingWheel.forwardFriction;
            robotWheelFrictionCurve.extremumSlip = 0.4f * frictionCoefficience;
            robotWheelFrictionCurve.extremumValue = 8f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteSlip = 0.8f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteValue = 5f * frictionCoefficience;
            axleInfos[i].FowaredHeadingWheel.forwardFriction = robotWheelFrictionCurve;

         /*   robotWheelFrictionCurve = axleInfos[i].FowaredHeadingWheel.sidewaysFriction;
            robotWheelFrictionCurve.extremumSlip = 0.2f * frictionCoefficience;
            robotWheelFrictionCurve.extremumValue = 0.5f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteSlip = 0.5f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteValue = 0.25f * frictionCoefficience;
            axleInfos[i].FowaredHeadingWheel.sidewaysFriction = robotWheelFrictionCurve;*/

            robotWheelFrictionCurve = axleInfos[i].SidewayRollingWheel.forwardFriction;
            robotWheelFrictionCurve.extremumSlip = 0.4f * frictionCoefficience;
            robotWheelFrictionCurve.extremumValue = 8f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteSlip = 0.8f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteValue = 5f * frictionCoefficience;
            axleInfos[i].SidewayRollingWheel.forwardFriction = robotWheelFrictionCurve;

          /*  robotWheelFrictionCurve = axleInfos[i].SidewayRollingWheel.sidewaysFriction;
            robotWheelFrictionCurve.extremumSlip = 0.2f * frictionCoefficience;
            robotWheelFrictionCurve.extremumValue = 0.5f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteSlip = 0.5f * frictionCoefficience;
            robotWheelFrictionCurve.asymptoteValue = 0.25f * frictionCoefficience;
            axleInfos[i].SidewayRollingWheel.sidewaysFriction = robotWheelFrictionCurve;*/
        }

    }
}