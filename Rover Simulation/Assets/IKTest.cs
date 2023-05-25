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

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 translatedPosition = new Vector3(new Vector2(target.position.x, target.position.z).magnitude * (target.position.z - origin.position.z >= 0 ? 1 : -1), target.position.y, origin.position.z);
        float zRot = Mathf.Atan2(target.position.z - origin.position.z, target.position.x - origin.position.x);
        float dist = Vector3.Distance(translatedPosition, origin.position);

        float yRot = Mathf.Atan2(translatedPosition.normalized.y, translatedPosition.normalized.x);


    }
}
