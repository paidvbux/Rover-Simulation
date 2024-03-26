using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePath : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject spawnObject;
    private Vector3 lastPosition;
    private float minDeltaDistance = 0.025f;
    private bool isDrawingEnabled;
    private float pointColor;

    // Start is called before the first frame update
    void Start()
    {
        isDrawingEnabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        pointColor = (Input.GetAxis("Draw") + 1.0f) / 2.0f;
        if (pointColor > 0.02)
        {
            if (isDrawingEnabled == false)
            {
                lastPosition = spawnPoint.position;
                isDrawingEnabled = true;
                StartCoroutine(PlotPoint());
            }
        }
        else
            isDrawingEnabled = false;
    }

    IEnumerator PlotPoint()
    {
        while (isDrawingEnabled)
        {

            float distanceToLastPoint = Vector3.Distance(spawnPoint.position, lastPosition);
            float segments = distanceToLastPoint / minDeltaDistance;
            
            

            if (segments > 4.0f)
            {
                
                for (int i = 0; i < (int)segments; i++)
                {
                    Vector3 spawnPosition = lastPosition + (spawnPoint.position - lastPosition) / segments * (float)(i + 1);
                    GameObject point = Instantiate(spawnObject, spawnPosition, spawnPoint.rotation);
                    point.GetComponent<Renderer>().material.color = Color.HSVToRGB(pointColor, 0.7f, 0.85f);
                    Destroy(point, 25);
                    lastPosition = spawnPosition;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
