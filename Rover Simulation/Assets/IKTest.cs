using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public Transform origin;
    public Transform joint;
    public Transform endJoint;
    public Transform target;

    public float linkLength1 = 0.75f;
    public float linkLength2 = 0.25f;
    void Update()
    {
        Vector3 targetPosition = new Vector3(new Vector2(target.position.x, target.position.z).magnitude, target.position.y) + origin.position;
        float zRot = Mathf.Atan2(target.position.z - origin.position.z, target.position.x - origin.position.x) * Mathf.Rad2Deg;
        float dist = Vector3.Distance(targetPosition, origin.position);

        dist = Mathf.Clamp(dist, Mathf.Abs(linkLength1 - linkLength2), linkLength1 + linkLength2);
        targetPosition = (targetPosition - origin.position).normalized * dist;

        float rot_1 = Mathf.Atan2(targetPosition.normalized.y, targetPosition.normalized.x);
        float rot_2 = Mathf.Acos(((dist * dist) + (linkLength1 * linkLength1) - (linkLength2 * linkLength2)) / (2 * dist * linkLength1)) + rot_1;

        Debug.DrawLine(origin.position, joint.position);
        Debug.DrawLine(joint.position, endJoint.position);

        if (dist < Mathf.Abs(linkLength1 - linkLength2))
            return;

        joint.position = new Vector3(Mathf.Cos(rot_2), Mathf.Sin(rot_2)) * linkLength1 + origin.position;
        endJoint.position = targetPosition;

        joint.RotateAround(origin.position, Vector3.up, -zRot);
        endJoint.RotateAround(origin.position, Vector3.up, -zRot);

        origin.up = (joint.position - origin.position).normalized;
        joint.up = (endJoint.position - joint.position).normalized;
    }
}