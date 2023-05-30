using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public Transform link1;
    public Transform link2;
    public Transform endEffector;
    public Transform target;

    public float linkLength1 = 0.75f;
    public float linkLength2 = 0.25f;

    void LateUpdate()
    {
        link1.GetChild(0).localScale = new Vector3(1, linkLength1, 1);
        link2.GetChild(0).localScale = new Vector3(1, linkLength2 - 0.25f, 1);

        link2.GetChild(1).localPosition = new Vector3(0, linkLength2 - 0.125f, 0);

        Vector3 targetPosition = new Vector3(new Vector2(target.position.x, target.position.z).magnitude, target.position.y) + link1.position;
        float zRot = Mathf.Atan2(target.position.z - link1.position.z, target.position.x - link1.position.x) * Mathf.Rad2Deg;
        float dist = Vector3.Distance(targetPosition, link1.position);

        dist = Mathf.Clamp(dist, Mathf.Abs(linkLength1 - linkLength2), linkLength1 + linkLength2);
        targetPosition = (targetPosition - link1.position).normalized * dist;

        float rot_1 = Mathf.Atan2(targetPosition.normalized.y, targetPosition.normalized.x);
        float rot_2 = Mathf.Acos(((dist * dist) + (linkLength1 * linkLength1) - (linkLength2 * linkLength2)) / (2 * dist * linkLength1)) + rot_1;

        if (dist < Mathf.Abs(linkLength1 - linkLength2))
            return;

        link2.position = new Vector3(Mathf.Cos(rot_2), Mathf.Sin(rot_2)) * linkLength1 + link1.position;
        endEffector.position = targetPosition;

        link2.RotateAround(link1.position, Vector3.up, -zRot);
        endEffector.RotateAround(link1.position, Vector3.up, -zRot);

        link1.up = (link2.position - link1.position).normalized;
        link2.up = (endEffector.position - link2.position).normalized;
    }
}