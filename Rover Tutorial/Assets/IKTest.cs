using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    [SerializeField] float[] thetaInitial;
    [SerializeField] Vector3 localPosInitial;
    [SerializeField] float[] alphas;
    [SerializeField] float[] zOffsets;
    [SerializeField] float[] xOffsets;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ComputeEndPosition(float[] thetas)
    {
        localPosInitial = Vector3.zero;
        for (int i = 0; i < thetas.Length; i++)
        {
            Vector3 newPos = localPosInitial;

        }
    }

    private void OnDrawGizmos()
    {
        ComputeEndPosition(thetaInitial);
    }
}
