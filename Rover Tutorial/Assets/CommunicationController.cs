using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunicationController : MonoBehaviour, IReceiverObserver
{
	UDPTransmitter UdpTransmitter;
	UDPReceiver UdpReceiver;

	[HideInInspector] public double[] sensorOutput;
	[HideInInspector] public double[] sensorInput;
	
	void Awake()
    {
		UdpReceiver = GetComponent<UDPReceiver>();
		UdpReceiver.observer = this;
		UdpTransmitter = GetComponent<UDPTransmitter>();
    }

	void Start()
	{
		int numOfOutputData = 27;
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
