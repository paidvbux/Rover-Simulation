using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MatlabController : MonoBehaviour
{
	public ArticulationBody robot;
	public Transform robotMountBase;
	public Transform endEffector;

	[SerializeField] Transform targetPos;

	CommunicationController controlSensorSignal;
	RobotController robotController;
	void Start()
	{
		robotController = robot.GetComponent<RobotController>();
		controlSensorSignal = GetComponentInChildren<CommunicationController>();
        //Set the robot to initial position 
        robotController.RotateJoint(0, 0);
        robotController.RotateJoint(1, -60);
        robotController.RotateJoint(2, 60);
        robotController.RotateJoint(3, -60);
        robotController.RotateJoint(4, 0);
    }
    void FixedUpdate()
	{
		robot.TeleportRoot(robotMountBase.position, robotMountBase.rotation);
		for (int i = 0; (i < robotController.joints.Length) && (i < controlSensorSignal.sensorInput.Length); i++)
		{
			robotController.RotateJoint(i, (float)controlSensorSignal.sensorInput[i]);
		}
		List<float> jointPositions = new List<float>();
		int size = robot.GetJointPositions(jointPositions);
		for (int i = 0; i < size && i < controlSensorSignal.sensorOutput.Length; i++)
		{
			controlSensorSignal.sensorOutput[i] = (double)jointPositions[i] * 180 / Math.PI;
		}

		controlSensorSignal.sensorOutput[18] = targetPos.position.x - robotController.origin.position.x;
		controlSensorSignal.sensorOutput[19] = targetPos.position.z - robotController.origin.position.z;
		controlSensorSignal.sensorOutput[20] = targetPos.position.y - robotController.origin.position.y;
		controlSensorSignal.sensorOutput[21] = robot.transform.position.x;
		controlSensorSignal.sensorOutput[22] = robot.transform.position.z;
		controlSensorSignal.sensorOutput[23] = robot.transform.position.y;
		controlSensorSignal.sensorOutput[24] = endEffector.position.x - robotController.origin.position.x;
		controlSensorSignal.sensorOutput[25] = endEffector.position.z - robotController.origin.position.z;
		controlSensorSignal.sensorOutput[26] = endEffector.position.y - robotController.origin.position.y;

	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(targetPos.position, endEffector.position);
    }
}
