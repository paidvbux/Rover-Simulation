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
    public ushort TransmitPort;
    private IPEndPoint _RemoteEndPoint;
    private UdpClient _TransmitClient;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Initialize objects.
    /// </summary>
    private void Initialize()
    {
        _RemoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), TransmitPort);
        _TransmitClient = new UdpClient();
    }

    /// <summary>
    /// Sends a double value to target port and ip.
    /// </summary>
    /// <param name="val"></param>
    public void Send(double val)
    {
        try
        {
            // Convert double into byte array (size of 8)  
            byte[] serverMessageAsByteArray = BitConverter.GetBytes(val);

            _TransmitClient.Send(serverMessageAsByteArray, serverMessageAsByteArray.Length, _RemoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }

    /// <summary>
    /// Sends a double array to target port and ip.
    /// </summary>
    /// <param name="val"></param>
    public void Send(double[] val)
    {
        try
        {
            int val_len = val.Length;
            byte[] byteTemp;
            int bLen = 8;
            byte[] serverMessageAsByteArray = new byte[val_len * bLen];

            for (int index = 0; index < val_len; index++)
            {   // Convert double into byte array (size of 8)
                byteTemp = BitConverter.GetBytes(val[index]);
                //constructing the Byte Array to be sent out
                for (int i = 0; i < bLen; i++)
                {
                    serverMessageAsByteArray[bLen * index + i] = byteTemp[i];
                }

            }
            _TransmitClient.Send(serverMessageAsByteArray, serverMessageAsByteArray.Length, _RemoteEndPoint);

        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }

    /// <summary>
    /// Deinitialize everything on quiting the application.Or you might get error in restart.
    /// </summary>
    private void OnApplicationQuit()
    {
        try
        {
            _TransmitClient.Close();
        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }
}
