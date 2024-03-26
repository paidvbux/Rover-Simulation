using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatlabControllerRockingBogie : MonoBehaviour
{
    
    private CommunicationController control_sensor_signal;

    [SerializeField] ArticulationBody[] wheelJoints; // Joints that turn the wheels
    [SerializeField] ArticulationBody[] wheels; // The wheels itself (not the mesh objects)
    public Transform rover_body;

    void Start()
    {
        control_sensor_signal = this.GetComponentInChildren<CommunicationController>();
        
    }

    void FixedUpdate()
    {
        // Matlab --> Unity

        string message = control_sensor_signal.sensor_input.Length + "; ";
        for(int i = 0; i < control_sensor_signal.sensor_input.Length; i++)
        {
            message += control_sensor_signal.sensor_input[i] + "; ";
        }

        Debug.Log(message);

        if (control_sensor_signal.sensor_input.Length < 10) return;

        // the first 4 items are for pod direction (Turning)
        for (int i = 0; i < wheelJoints.Length; i++)
        {
            wheelJoints[i].GetComponent<WheelTurnOffset>().SetAngle((float)control_sensor_signal.sensor_input[i]);
        }

        // item 5-6 are not used, the 6-10 are wheel velocity in RPM
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetComponent<TempWheelScript>().RotateWheel((float)control_sensor_signal.sensor_input[i+6] * 6); //RPM -> Degree/second = 360/60=6
            ArticulationReducedSpace jointVelocity = wheels[i].jointVelocity;
            //control_sensor_signal.sensor_output[1+i] = (double) wheels[i].jointVelocity[2];

            //wheels[i].AddTorque(Vector3.forward * (float)control_sensor_signal.sensor_input[i+5]);
        }

        //Return the robot orientation
        control_sensor_signal.sensor_output[0] = - rover_body.rotation.eulerAngles.y ; //unity positive turning cw, and our robot math formula is : CCW positive

       
        
        
        // Unity --> Matlab

        //for (int i = 0; i < wheelJoints.Length; i++)
        //{
        //    List<float> jointPositions = new List<float>();
        //    wheelJoints[i].GetJointPositions(jointPositions);
        //    control_sensor_signal.sensor_output[i] = jointPositions[0];
        //}

        //for (int i = 0; i < wheels.Length; i++)
        //{
        //    control_sensor_signal.sensor_output[i+4] = wheels[i+4].angularVelocity.z;
        //}

        // control_sensor_signal.sensor_output[9] = control_sensor_signal.sensor_input[9]; //send back the time to estimate the time delay.

        //control_sensor_signal.sensor_output[10] = Input.GetAxis("ArmUp");
        //control_sensor_signal.sensor_output[11] = Input.GetAxis("ArmLeft");
        //control_sensor_signal.sensor_output[12] = Input.GetAxis("ArmIn");
        //control_sensor_signal.sensor_output[13] = Input.GetAxis("ArmTilt");
        //control_sensor_signal.sensor_output[14] = Input.GetAxis("ArmPan");
        //control_sensor_signal.sensor_output[15] = Input.GetAxis("ArmRoll");

        //Sensor output: [0:8] joint rotation readings; [9] Received Time; [10:15] joysticks commands 

    }
}
