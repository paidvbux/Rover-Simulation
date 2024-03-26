using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTurnOffset : MonoBehaviour
{
    [SerializeField] float angleOffset;
    [SerializeField] bool reverse;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAngle(float angle)
    {
        var drive = GetComponent<ArticulationBody>().xDrive;
        drive.target = angle * (reverse ? -1 : 1) + angleOffset;
        GetComponent<ArticulationBody>().xDrive = drive;
    }
}
