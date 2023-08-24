using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RobotController : MonoBehaviour
{
    public ArticulationJointController[] joints;

    [SerializeField] Transform origin;
    [SerializeField] Transform endpoint;

    public void RotateJoint(int jointIndex, float targetRotation)
    {
        joints[jointIndex].targetRotation = targetRotation;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 pos = new Vector3(origin.position.x, origin.position.y, endpoint.position.z);
        Gizmos.DrawLine(origin.position, pos);

        Gizmos.color = Color.green;
        Vector3 pos1 = new Vector3(pos.x, endpoint.position.y, pos.z);
        Gizmos.DrawLine(pos1, pos);

        Gizmos.color = Color.red;
        pos = new Vector3(endpoint.position.x, pos1.y, pos1.z);
        Gizmos.DrawLine(pos1, pos);
    }
}

