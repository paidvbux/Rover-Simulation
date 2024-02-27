using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverScript : MonoBehaviour
{
    [SerializeField] ArticulationBody root;
    [SerializeField] MotorScript[] legs;

    [SerializeField] MotorScript[] swerveMotors;
    [SerializeField] MotorScript[] wheelMotors;

    [SerializeField] float speed;

   /*
    index 0 -> right front
    index 1 -> left front
    index 2 -> right rear
    index 3 -> left rear
    */

    void Start()
    {
        
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * speed;
        float vertical = Input.GetAxisRaw("Vertical") * speed;

        for (int i = 0; i < swerveMotors.Length; i++)
            swerveMotors[i].rotateTo(horizontal, true);
        
        for (int i = 0; i < wheelMotors.Length; i++)
            wheelMotors[i].rotateTo(vertical, true);
    }
}
