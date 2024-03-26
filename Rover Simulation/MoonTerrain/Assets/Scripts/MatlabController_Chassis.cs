using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MatlabController_Chassis : MonoBehaviour
{
    private MecanumWheel robotChassis;
    private CommunicationController control_sensor_signal;
   

    void Start()
    {
        //robotController = robot.GetComponent<RobotController>();
        robotChassis = GetComponent<MecanumWheel>();
        control_sensor_signal = this.GetComponentInChildren<CommunicationController>();

    }


    void FixedUpdate()
    {

        if (control_sensor_signal.sensor_input.Length >2)
        {
        robotChassis.typeOfOperation = (int) control_sensor_signal.sensor_input[0];
        robotChassis.Vx= (float) control_sensor_signal.sensor_input[1];
        robotChassis.Vy = (float)control_sensor_signal.sensor_input[2]; 
        robotChassis.Vturn = (float)control_sensor_signal.sensor_input[3];


        robotChassis.surfaceFriction((float) control_sensor_signal.sensor_input[4]);

        control_sensor_signal.sensor_output[0] = robotChassis.robotBody.transform.position.z; //Global X cordinate 
        control_sensor_signal.sensor_output[1] = -robotChassis.robotBody.transform.position.x; //Gloabl Y cordinate
        control_sensor_signal.sensor_output[2] = robotChassis.globalAngle; // Heading of Robot


        }



        /*
        for (int i = 0; (i < robotController.joints.Length) && (i < control_sensor_signal.sensor_input.Length); i++)
        {
            robotController.RotateJoint(i, (float)control_sensor_signal.sensor_input[i]);

        }
        List<float> jointPositions = new List<float>();

        int size = articulation.GetJointPositions(jointPositions);

        for (int i = 0; i < size && i < control_sensor_signal.sensor_output.Length; i++)
        {
            control_sensor_signal.sensor_output[i] = (double)jointPositions[i] * 180 / 3.1415;
            //Debug.Log(jointPositions[i]);
        }

         */

    }

}
