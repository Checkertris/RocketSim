using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RocketInfo : MonoBehaviour
{
    //Others
    public TextMeshProUGUI infoText;
    public Universe universe;
    private Rigidbody rb;
    public Transform directionalLight;

    //RocketInfo
    private string currentPlanetName;
    private string currentPlanetClass;
    public float gravity;
    private string atmosphere;
    public float height;


    void Start()
    {
        //Settings
        currentPlanetName = "Earth";
        universe = FindObjectOfType<Universe>();
        //Constants
        // currentPlanetClass = Array.Find(planets, planet => planet.name == currentPlanetName);
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        SetHeight();
        CalcGravity();
        UpdateText();

        CheckAtmosphere();

    }

    void SetHeight()
    {
        height = transform.position.y;
        //height = Vector3.distance(transform.position, currentPlanetName.obj.transform.position) - earthradius * actual earth radius 6 something km;
    }

    void AdjustLight()
    {
        float x = 130f + (height / 80000f) * 66f;
        directionalLight.rotation = Quaternion.Euler(x, -209.822f, -180.851f);
    }

    void CheckAtmosphere()
    {

        if (0 <= height && height < 20000)
        {
            atmosphere = "Troposphere";
            AdjustLight();
        }
        else if (20000 <= height && height < 50000)
        {
            atmosphere = "Stratosphere";
            AdjustLight();
        }
        else if (50000 <= height && height < 85000)
        {
            atmosphere = "Mesosphere";
            AdjustLight();
        }
        else if (85000 <= height && height < 690000)
        {
            atmosphere = "Thermosphere";
            AdjustLight();
        }
        else if (690000 <= height && height < 10000000)
        {
            atmosphere = "Exosphere";
            AdjustLight();
        }

        else if (10000000 <= height)
        {
            atmosphere = "Space";
        }

    }

    void UpdateText()
    {
        infoText.text = "PLANET: " + currentPlanetName + "\n\nVELOCITY: " + (int)rb.velocity.magnitude + "m/s" + "\n\nALTITUDE: " + (int)(0.001 * height) + " km" + "\n\nGRAVITY: " + Mathf.Round(gravity * 100f) / 100f + " m/s^2" + "\n\nATMOSPHERE: " + atmosphere;
    }

    void CalcGravity()
    {
        float earthRadius = 1f + height / 6371000f;
        gravity = 9.81f / (earthRadius * earthRadius);
        //gravity = 9.81f * currentPlanetClass.earthGravity / (earthRadius * earthRadius);

    }
}
