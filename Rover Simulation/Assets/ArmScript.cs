using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArticulationBody))]
public class ArmScript : MonoBehaviour
{
    [SerializeField] ArticulationBody arm => GetComponent<ArticulationBody>();
    [SerializeField] ArticulationBody[] armBodies;
    [SerializeField] Transform armMount;
    [SerializeField] float armLength_1 = 1f;
    [SerializeField] float armLength_2 = 1f;
    [SerializeField] Transform IKTarget;

    void LateUpdate()
    {
        arm.TeleportRoot(armMount.position, armMount.rotation);
    }

    void Update()
    {
        CalculateArmPositionFromPoint(IKTarget.position);

        //if (Input.GetKey(KeyCode.E))
        //{
        //    temp = armBodies[0].yDrive;
        //    temp.target += 45 * Time.deltaTime;

        //    armBodies[0].yDrive = temp;
        //}
        //else if (Input.GetKey(KeyCode.R))
        //{
        //    temp = armBodies[0].yDrive;
        //    temp.target -= 45 * Time.deltaTime;

        //    armBodies[0].yDrive = temp;
        //}
        //if (Input.GetKey(KeyCode.N))
        //{
        //    temp = armBodies[1].zDrive;
        //    temp.target += 45 * Time.deltaTime;
        //    temp.target = Mathf.Clamp(temp.target, -90, 90);

        //    armBodies[1].zDrive = temp;
        //}
        //else if (Input.GetKey(KeyCode.M))
        //{
        //    temp = armBodies[1].zDrive;
        //    temp.target -= 45 * Time.deltaTime;
        //    temp.target = Mathf.Clamp(temp.target, -90, 90);

        //    armBodies[1].zDrive = temp;
        //}
        //if (Input.GetKey(KeyCode.V))
        //{
        //    temp = armBodies[2].zDrive;
        //    temp.target += 45 * Time.deltaTime;
        //    temp.target = Mathf.Clamp(temp.target, -160, 160);

        //    armBodies[2].zDrive = temp;
        //}
        //else if (Input.GetKey(KeyCode.B))
        //{
        //    temp = armBodies[2].zDrive;
        //    temp.target -= 45 * Time.deltaTime;
        //    temp.target = Mathf.Clamp(temp.target, -160, 160);

        //    armBodies[2].zDrive = temp;
        //}
    }

    void CalculateArmPositionFromPoint(Vector3 position)
    {
        float dist = Vector3.Distance(position, armMount.position);

        if (dist <= armLength_1 + armLength_2)
        {
            //Vector3 normalizedVector = (position - armMount.position).normalized;
            //float theta1 = Mathf.Atan2(normalizedVector.y, normalizedVector.x);

            //float cosAngle = (dist * dist + armLength_1 * armLength_1 - armLength_2 * armLength_2) / (2 * dist * armLength_1);
            //float theta2 = Mathf.Acos(cosAngle);

            //armBodies[1].zDrive = ChangeTarget(armBodies[1].zDrive, theta1 * Mathf.Rad2Deg);
            //armBodies[2].zDrive = ChangeTarget(armBodies[2].zDrive, theta2 * Mathf.Rad2Deg);

            armBodies[1].zDrive = ChangeTarget(armBodies[1].zDrive, 90 * Mathf.Sin(Time.time));
            armBodies[2].zDrive = ChangeTarget(armBodies[2].zDrive, 90 * Mathf.Cos(Time.time));
            armBodies[3].transform.localPosition = new Vector3(0, 0.5f * Mathf.Sin(Time.time) + 0.5f, 0);
        }
    }

    ArticulationDrive ChangeTarget(ArticulationDrive current, float target)
    {
        ArticulationDrive temp = current;
        temp.target = target;
        return temp;
    }
}

