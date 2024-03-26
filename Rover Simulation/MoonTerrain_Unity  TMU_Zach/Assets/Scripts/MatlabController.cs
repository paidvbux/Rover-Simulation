using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class MatlabController: MonoBehaviour
{
    public GameObject robot;
    private RobotController robotController;
    private CommunicationController control_sensor_signal;
    private ArticulationBody articulation;
    public ArticulationBody armBase;
    public Transform robotMountbase;

    void Start()
    {
        robotController = robot.GetComponent<RobotController>();
        control_sensor_signal = this.GetComponentInChildren<CommunicationController>();
        articulation = FindObjectOfType<ArticulationBody>();
        // move the robot to initial position

        robotController.RotateJoint(0, 0);
        robotController.RotateJoint(1, 45);
        robotController.RotateJoint(2, -75);
        robotController.RotateJoint(3, 0);// fixed at 0
        robotController.RotateJoint(4, 120); 
        robotController.RotateJoint(5, 90);
        robotController.RotateJoint(6, 0);

    }


    void FixedUpdate()
    {
        armBase.TeleportRoot(robotMountbase.position, robotMountbase.rotation);

        for (int i = 0; (i < robotController.joints.Length) && (i< control_sensor_signal.sensor_input.Length); i++)
        {   
                robotController.RotateJoint(i, (float) control_sensor_signal.sensor_input[i]);    
            
        }
        List<float> jointPositions = new List<float>();

        int size = articulation.GetJointPositions(jointPositions);
         
       for (int i=0; i<size && i < control_sensor_signal.sensor_output.Length; i++)
        {
            control_sensor_signal.sensor_output[i] = (double) jointPositions[i] * 180/3.1415;
            //Debug.Log(jointPositions[i]);
        }

        control_sensor_signal.sensor_output[9] = control_sensor_signal.sensor_input[9];//send back the time to estimate the time delay.

        
        

        control_sensor_signal.sensor_output[10] = Input.GetAxis("ArmUp");
        control_sensor_signal.sensor_output[11] = Input.GetAxis("ArmLeft");
        control_sensor_signal.sensor_output[12] = Input.GetAxis("ArmIn");
        control_sensor_signal.sensor_output[13] = Input.GetAxis("ArmTilt");
        control_sensor_signal.sensor_output[14] = Input.GetAxis("ArmPan");
        control_sensor_signal.sensor_output[15] = Input.GetAxis("ArmRoll");

    }

}
