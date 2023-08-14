using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    public ushort port = 28007;
    public UdpClient receiveClient;
    public Thread receiveThread;
    public IReceiverObserver observer;

    public void Initialize()
    {
        // Receive
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    void ReceiveData()
    {
        receiveClient = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = receiveClient.Receive(ref anyIP);
                double[] values = new double[data.Length / 8];
                Buffer.BlockCopy(data, 0, values, 0, values.Length * 8);
                if (observer != null)
                    observer.OnDataReceived(values);
            }
            catch (Exception err)
            {
                Debug.Log("<color=red>" + err.Message + "</color>");
            }
        }
    }
}

