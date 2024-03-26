using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocker_Bogie_Movement : MonoBehaviour
{
    [SerializeField] WheelCollider[] wheelColliders;
    [SerializeField] Transform[] wheelModels;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = speed;
            
        }

        UpdateVisualModels();
    }

    void UpdateVisualModels()
    {
        for(int i = 0; i < wheelColliders.Length; i++)
        {
            Vector3 position;
            Quaternion rotation;
            wheelColliders[i].GetWorldPose(out position, out rotation);
            wheelModels[i].position = position;
            wheelModels[i].rotation = rotation;
        }
    }
}
