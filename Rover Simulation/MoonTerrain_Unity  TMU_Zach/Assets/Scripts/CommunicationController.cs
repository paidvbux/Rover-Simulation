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

    //[System.Serializable]
    //public struct RotationAngles
    //{
    //    public double[] thetas;

    //    public RotationAngles(double[] _thetas)
    //    {
    //        thetas = _thetas;
    //    }
    //}

    //[SerializeField]
    //List<RotationAngles> allValues = new List<RotationAngles>();
    
    private void Awake()
    {
        _UdpReceiver = GetComponent<UDPReceiver>();
        _UdpReceiver.SetObserver(this);
        _Udpransmitter = GetComponent<UDPTransmitter>();
       
    }
    private void Start()
    {
        int number_of_outputData = 21;
        sensor_output = new double[number_of_outputData];
    }

    void IReceiverObserver.OnDataReceived(double[] val)
    {
        sensor_input = val;
        //allValues.Add(new RotationAngles(val));
    }

    void FixedUpdate()
    {
        _Udpransmitter.Send(sensor_output);
    }

}
