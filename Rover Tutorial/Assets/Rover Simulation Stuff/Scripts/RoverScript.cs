using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverScript : MonoBehaviour
{
    [SerializeField] Transform robotOrigin;
    [SerializeField] Transform displayOrigin;
    [SerializeField] Transform[] legs;

    void Start()
    {
        
    }

    void Update()
    {
        displayOrigin.localEulerAngles = new Vector3(0, robotOrigin.localEulerAngles.y, robotOrigin.localEulerAngles.z);
        legs[0].localEulerAngles = new Vector3(legs[0].localEulerAngles.x - robotOrigin.localEulerAngles.x, 
                                               legs[0].localEulerAngles.y, 
                                               legs[0].localEulerAngles.z);
        legs[1].localEulerAngles = new Vector3(legs[1].localEulerAngles.x - robotOrigin.localEulerAngles.x, 
                                               legs[1].localEulerAngles.y, 
                                               legs[1].localEulerAngles.z);
    }
}
