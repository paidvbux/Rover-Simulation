using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CommunicationController : MonoBehaviour, IReceiverObserver
{
    UDPReceiver _UdpReceiver;
    UDPTransmitter _Udpransmitter;
    

    [HideInInspector]
    public double[] sensor_input ;
    [HideInInspector]
    public double[] sensor_output;
    


    private void Awake()
    {
        _UdpReceiver = GetComponent<UDPReceiver>();
        _UdpReceiver.SetObserver(this);
        _Udpransmitter = GetComponent<UDPTransmitter>();
       
    }
    private void Start()
    {
        int number_of_outputData = 9;
        sensor_output = new double[number_of_outputData];
    }

    /// <summary>
    /// Send data immediately after receiving it.
    /// </summary>
    /// <param name="val"></param>
    void IReceiverObserver.OnDataReceived(double[] val)
    {
    
        sensor_input = val;
    //instead using FixedUpdate, here send data back whenever it receives a data coming in    
         
            _Udpransmitter.Send(sensor_output);
         
    }

    void FixedUpdate()
    {
        //_Udpransmitter.Send(sensor_output);
      
    }

}
