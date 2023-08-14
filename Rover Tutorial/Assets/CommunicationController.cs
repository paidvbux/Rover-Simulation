using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationController : MonoBehaviour, IReceiverObserver
{
	public double[] sensorOutput;
	public double[] sensorInput;
	public UDPTransmitter UdpTransmitter;
	void Start()
	{
		int numOfOutputData = 18;
		sensorOutput = new double[numOfOutputData];
	}

	void IReceiverObserver.OnDataReceived(double[] val)
	{
		sensorInput = val;
	}

	void FixedUpdate()
	{
		UdpTransmitter.Send(sensorOutput);
	}
}
