using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class UDPTransmitter : MonoBehaviour
{
	public string IP = "127.0.0.1";
	public ushort transmitPort = 28008;
	IPEndPoint remoteEndPoint;
	UdpClient transmitClient;

    void Start()
    {
		remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), transmitPort);
		transmitClient = new UdpClient();
    }

	public void Send(double val)
    {
		try
		{
			// Convert double into byte array (size of 8)  
			byte[] serverMessageAsByteArray = BitConverter.GetBytes(val);

			transmitClient.Send(serverMessageAsByteArray, serverMessageAsByteArray.Length, remoteEndPoint);
		}
		catch (Exception err)
		{
			Debug.Log($"<color=red>{err.Message}</color>");
		}
	}

    public void Send(double[] val)
	{
		try
		{
			int valLen = val.Length;
			byte[] byteTemp;
			int bLen = 8;
			byte[] serverMessage = new byte[valLen * bLen];
			val[0] *= -1;
			val[5] *= -1;
			for (int i = 0; i < valLen; i++)
			{
				//Convert double values into byte array of length 8
				byteTemp = BitConverter.GetBytes(val[i]);
				//Constructing the byte array to be sent
				for (int j = 0; j < bLen; j++)
				{
					serverMessage[bLen * i + j] = byteTemp[j];
				}
			}
			transmitClient.Send(serverMessage, serverMessage.Length, remoteEndPoint);
		}
		catch (Exception err)
		{
			Debug.LogError(err.Message);
		}
	}

	private void OnApplicationQuit()
	{
		try
		{
			transmitClient.Close();
		}
		catch (Exception err)
		{
			Debug.Log($"<color=red>{err.Message}</color>");
		}
	}
}
