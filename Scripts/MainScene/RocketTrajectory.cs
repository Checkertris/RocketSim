using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class RocketTrajectory : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastVel;

//Retreiving Info
    private RocketInfo rocketInfo;
    public ControlPanelInfo controlPanel;

    private Vector3[] dataPoints;
    private int initialLength;
    private int range;
    private float stepSize;

    public Material lineMaterial;
    private LineRenderer lineRenderer;


    void Start()
    {
        //Set Up GenerateGraph
        CreateObject("LineRenderer", Color.green, gameObject);
        initialLength = 10000;

        //Constants
        rb = GetComponent<Rigidbody>();
        rocketInfo = GetComponent<RocketInfo>();
        stepSize = 0.1f;
        lastVel = new Vector3(0f, 0f, 0f);
        controlPanel = FindObjectOfType<ControlPanelInfo>();

    }


    void Update()
    {
  
         if (controlPanel.GetViewMode())
            {
                   lineRenderer.positionCount = 0;
            }

            else
            {
          lineRenderer.SetWidth(0.04f * controlPanel.GetZoom(), 0.04f * controlPanel.GetZoom());
            GenerateGraph();

            }
    }


    void GenerateGraph()
   {
       //Constants
        Vector3[] temp = new Vector3[initialLength];
        float a = rocketInfo.gravity;

        for (int i = 0; i < initialLength; i++)
        {
            //Constants
            float t = i * stepSize;
            float half_t_squared = t * t * 0.5f;
            
            //Calculations
            Vector3 A = (rb.velocity - lastVel) / Time.fixedDeltaTime;
            lastVel = rb.velocity;
            Vector3 acceleration = new Vector3(A.x, A.y - a, A.z);
            Vector3 newVec = transform.position + (t*rb.velocity) + (half_t_squared * acceleration);
            
            if (newVec.y < 0) {
                range = i;
                break;
            }
            
            temp[i] = newVec;
        }

        // Reassign data points the new range
        dataPoints = new Vector3[range];
        lineRenderer.positionCount = dataPoints.Length;

        for (int i = 0; i < dataPoints.Length; i++)
        {
            // Manual array copy
            dataPoints[i] = temp[i];
            lineRenderer.SetPosition(i, temp[i]);
        }
    }

    void CreateObject(string name, Color color, GameObject parent)
    {
        GameObject newObject = new GameObject(name);
        lineRenderer = newObject.AddComponent<LineRenderer>();

        lineRenderer.material = lineMaterial;
        lineRenderer.numCornerVertices = 30;
        lineRenderer.numCapVertices = 30;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material.SetColor("_Color", color);

        Transform targetTransform = parent.transform;
        newObject.transform.SetParent(targetTransform);
    }

}
