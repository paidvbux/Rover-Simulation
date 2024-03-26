using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverBodyScript : MonoBehaviour
{

    ArticulationBody articulation;

    // Start is called before the first frame update
    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
