using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MatlabControllerV2 : MonoBehaviour
{
    public GameObject robot;
    private RobotControllerV2 robotController;
    private CommunicationController control_sensor_signal;
    private ArticulationBody articulation;
    public ArticulationBody armBase;
    public Transform robotMountbase;
    public Transform target;
    public Transform endEffector;

    void Start()
    {
        robotController = robot.GetComponent<RobotControllerV2>();
        control_sensor_signal = GetComponentInChildren<CommunicationController>();
        articulation = FindObjectOfType<ArticulationBody>();
        // move the robot to initial position

        robotController.RotateJoint(0, 18);
        robotController.RotateJoint(1, 36);
        robotController.RotateJoint(2, 30);
        robotController.RotateJoint(3, 45);
        robotController.RotateJoint(4, 60);
        robotController.RotateJoint(5, 18);

        //robotController.RotateJoint(0, 0);
        //robotController.RotateJoint(1, 0);
        //robotController.RotateJoint(2, 0);
        //robotController.RotateJoint(3, 0);
        //robotController.RotateJoint(4, 0);
        //robotController.RotateJoint(5, 0);
    }


    void FixedUpdate()
    {
        //armBase.TeleportRoot(robotMountbase.position, robotMountbase.rotation);

        for (int i = 0; (i < robotController.joints.Length) && (i < control_sensor_signal.sensor_input.Length); i++)
        {
            robotController.RotateJoint(i, (float)control_sensor_signal.sensor_input[i]);

        }
        List<float> jointPositions = new List<float>();

        int size = articulation.GetJointPositions(jointPositions);

        for (int i = 0; i < size && i < control_sensor_signal.sensor_output.Length; i++)
        {
            control_sensor_signal.sensor_output[i] = (double)jointPositions[i] * 180 / 3.1415;
        }

        control_sensor_signal.sensor_output[9] = target.position.x / 100;
        control_sensor_signal.sensor_output[10] = target.position.z / 100;
        control_sensor_signal.sensor_output[11] = target.position.y / 100;
        control_sensor_signal.sensor_output[12] = target.eulerAngles.x;
        control_sensor_signal.sensor_output[13] = target.eulerAngles.z;
        control_sensor_signal.sensor_output[14] = target.eulerAngles.y;
        
        control_sensor_signal.sensor_output[15] = endEffector.position.x / 100;
        control_sensor_signal.sensor_output[16] = endEffector.position.z / 100;
        control_sensor_signal.sensor_output[17] = endEffector.position.y / 100;
        control_sensor_signal.sensor_output[18] = endEffector.eulerAngles.x;
        control_sensor_signal.sensor_output[19] = endEffector.eulerAngles.z;
        control_sensor_signal.sensor_output[20] = endEffector.eulerAngles.y;
        

        //control_sensor_signal.sensor_output[9] = control_sensor_signal.sensor_input[9];//send back the time to estimate the time delay.

        //control_sensor_signal.sensor_output[10] = Input.GetAxis("ArmUp");
        //control_sensor_signal.sensor_output[11] = Input.GetAxis("ArmLeft");
        //control_sensor_signal.sensor_output[12] = Input.GetAxis("ArmIn");
        //control_sensor_signal.sensor_output[13] = Input.GetAxis("ArmTilt");
        //control_sensor_signal.sensor_output[14] = Input.GetAxis("ArmPan");
        //control_sensor_signal.sensor_output[15] = Input.GetAxis("ArmRoll");
    }

}
