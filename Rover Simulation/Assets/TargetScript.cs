using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public bool usePath;

    void Update()
    {
        if (!usePath)
            return;

        float t = Time.time % (2 * Mathf.PI);

        transform.position = new Vector3(
            Mathf.Cos(t) * Mathf.Sin(3 * t) + 2,
            Mathf.Sin(t) * Mathf.Sin(3 * t) + 2,
            Mathf.Sin(6 * t) / 4 + 3
            );
    }
}